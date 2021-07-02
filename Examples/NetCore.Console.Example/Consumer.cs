using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shield.Client;
using Shield.Client.Extensions;

namespace NetCore.Console.Example
{
    public class Consumer
    {
        private readonly ILogger _logger;

        public Consumer(ILogger<Consumer> logger)
        {
            _logger = logger;
        }

        internal async Task Run()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            const string directory =
                @"C:\Users\juana\source\repos\Ejemplo\bin\Debug\netcoreapp3.1\";

            var appPath = $"{directory}Ejemplo.dll";

            var save = $"{directory}Ejemplo_protegido.dll";

            var dependencies = Directory.GetFiles($"{directory}").ToList();

            var client = ShieldClient.CreateInstance(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE4ODgzMmEyLTUxODktNDMwZS05NGFmLTc3MTJkZTBiM2FmZCIsInVuaXF1ZV9uYW1lIjoiOTE4ZDgxNmYtZDI4Zi00YThjLWE3MWItMzZiM2VkYTdlNjY4IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6ImFjOGRjMzA1LTNhNTEtNGQ0OC1iYTM2LTQ3NTVjYTYzYWEzMSIsImV4cCI6MTYyNTAwNzAxOH0.14XV2lcAoByRESSwC5D_DixJldKeRcE2d0pOigEHINo"
                , _logger);

            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("arg3");

            var uploadApplicationDirectly = await client.Application.UploadApplicationDirectlyAsync(projectTest.Key, appPath, null);

            if (uploadApplicationDirectly.RequiresDependencies ||
                string.IsNullOrEmpty(uploadApplicationDirectly.ApplicationBlob))
                throw new Exception("Se necesitan dependencias.");

            var appBlob = uploadApplicationDirectly.ApplicationBlob;

            var connection = client.Connector.CreateHubConnection();

            var taskConnection = await client.Connector.InstanceHubConnectorWithLoggerAsync(connection);

            await taskConnection.StartAsync();

            var config = client.Configuration.FindApplicationConfiguration(
                             directory,
                Path.GetFileName(appPath)) ??
                         client.Configuration.MakeApplicationCustomConfiguration("consts mutation");

            var protect = await client.Tasks.ProtectSingleFileAsync(
                projectTest.Key,
                appBlob,
                connection,
                config);



            protect.OnError(taskConnection, error => _logger.LogCritical(error));

            protect.OnClose(taskConnection, _ => _logger.LogDebug("Connection close. Task finished."));

            protect.OnSuccess(taskConnection, async dto =>
                await (await client.Application.DownloadApplicationAsArrayAsync(dto))
                .SaveOnAsync(save, true)
                .ContinueWith(_ => _logger.LogInformation($"La aplicación protegida ha sido guardada en {save}"))
            );

            System.Console.ReadKey();
        }

        internal async Task RunWithQueues()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            const string directory =
                @"C:\Users\juana\source\repos\Ejemplo\bin\Debug\netcoreapp3.1\";

            var appPath = $"{directory}Ejemplo.dll";

            var save = $"{directory}Ejemplo_protegida.dll";

            var dependencies = Directory.GetFiles($"{directory}").ToList();

            var client = ShieldClient.CreateInstance(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE4ODgzMmEyLTUxODktNDMwZS05NGFmLTc3MTJkZTBiM2FmZCIsInVuaXF1ZV9uYW1lIjoiOTE4ZDgxNmYtZDI4Zi00YThjLWE3MWItMzZiM2VkYTdlNjY4IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6IjY5YTlkNjdiLWM4ZTgtNGNhYS05MWM3LTk5NDIwZGE2ZDU5YyIsImV4cCI6MTYxNzQwMjg1Mn0.Ohr4WeJaU5w_2CP1QhzAepis_xKmDheLYxz4BN2rLEo"
                , _logger);

            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("hola");

            var uploadApplicationDirectly = await client.Application.UploadApplicationDirectlyAsync(projectTest.Key, appPath, null);

            if (uploadApplicationDirectly.RequiresDependencies)
            {
                uploadApplicationDirectly.RequiredDependencies.ForEach(dependency =>
                    _logger.LogCritical($"Dependency ${dependency} is required."));
                throw new Exception("Some dependencies are required.");
            }

            if (string.IsNullOrEmpty(uploadApplicationDirectly.ApplicationBlob))
                throw new Exception("Unknown exception uploading application.");

            var appBlob = uploadApplicationDirectly.ApplicationBlob;

            var connection = client.Connector.CreateQueueConnection();

            var taskConnection = client.Connector.InstanceQueueConnectorWithLogger(connection);

            await taskConnection.StartAsync();

            var config = client.Configuration.FindApplicationConfiguration(
                             directory,
                             Path.GetFileName(appPath)) ??
                         client.Configuration.MakeApplicationCustomConfiguration("consts mutation");

            var protect = await client.Tasks.ProtectSingleFileAsync(
                projectTest.Key,
                appBlob,
                connection,
                config,
                connection.OnLogger);

            protect.OnError(taskConnection, error => _logger.LogCritical(error));

            protect.OnClose(taskConnection, _ => _logger.LogDebug("Connection close. Task finished."));

            protect.OnSuccess(taskConnection, async dto =>
                await (await client.Application.DownloadApplicationAsArrayAsync(dto))
                .SaveOnAsync(save, true)
                .ContinueWith(_ => _logger.LogInformation($"La aplicación protegida ha sido guardada en {save}"))
            );

            System.Console.ReadKey();
        }

        internal async Task RunWithSse()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            var client = ShieldClient.CreateInstance(
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5MWVkZjU4OC1iMmJhLTQ3NDAtOTJkYy1mY2RkMjFmMTI5M2YiLCJqdGkiOiI1ZDJlYTQ4OTk4ZmNmYzM0ZTQ0ODJhMTgwNmNlZTRmNDQ4YjlmN2YzMjUyYTUyY2ZmYmFkZTJkMzc2Yjg4NzM0YThmYWQ4OTVhN2ViYWY0NCIsImlhdCI6MTYyNTI2MDc4NywibmJmIjoxNjI1MjYwNzg3LCJleHAiOjE2NTY3OTY3ODYsInN1YiI6IjkxZWRmMDAzLWVlNjktNDU1Yy1hZWU4LWZlMzQ1ODZhOTdkNiIsInNjb3BlcyI6W119.GKbGITxU_UGSDYC3TA1BMycwgUl08DE2Fcprn-9TxitLIdvlRVsYGFcgE_uM8mLGeYLgwYsuRDahwfSE71u5DueiVpNWDNF49htUOLOTtZ7UjbotDi2UhKUq3NCa4_XwpbFbo9URn7Ld9E9UbU_BCcJBdMpQ4t45xQI3Lwv5dbUW4kQ9RAz2GA_9LPQu-oG4aLNnO--rZdZmlFDmUe5uTJpifeD2KLLGb6_BIbL5FXwScwof2WCTJ2l2h_jas7BjnBM7T9dK5S7LItvJ_1fYVsckZ9-q4ntP2crb45FkWunBg4p1ivq3lfLdyPmDPJqGYuk4B3Oz53vb2UTyiVNimbm2kD2WxeRNqepJQ-WI3A4kLOcrEaQl0xu5Y3c1xn25suc7TigyPP6Lxyac0qfrTFNv1NRnUogo7CJKGjEK09fypX0xn0R-demEVjqkFvUT2ncW5zEDtGhoP76yvf7UyNsioz7NCloaaKHcNT8AloCCLAZTgKWkDAiePmknydQ60ezeyM6E_cTIu8MOvZ95eAi_wUJN_Vk9T_VzakXXZKHjiXipcWyFY-ceE3SJx4yWewp3kOjFkvkGdYaSQ7Nwz0M_rW02wzxEVh90wX83yv228zp6mc24SLVuLJpqg-Wc89CCp7CvK71Fhzu2F3mxunvzW0Z8jyj9Tj4BBCz880A"
                , _logger);


            const string directory =
                @"C:\Users\juana\source\repos\Ornitorrinco\Ornitorrinco\bin\Debug\netcoreapp3.1\";

            var appPath = $"{directory}Ornitorrinco.dll";

            var save = $"{directory}Ornitorrinco_protegida.dll";

            var dependencies = Directory.GetFiles($"{directory}").ToList();

           
            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("hola");

            var uploadApplicationDirectly = await client.Application.UploadApplicationDirectlyAsync(projectTest.Key, appPath, null);

            if (uploadApplicationDirectly.RequiresDependencies)
            {
                uploadApplicationDirectly.RequiredDependencies.ForEach(dependency =>
                    _logger.LogCritical($"Dependency ${dependency} is required."));
                throw new Exception("Some dependencies are required.");
            }

            if (string.IsNullOrEmpty(uploadApplicationDirectly.ApplicationBlob))
                throw new Exception("Unknown exception uploading application.");

            var appBlob = uploadApplicationDirectly.ApplicationBlob;


            var config = client.Configuration.FindApplicationConfiguration(
                             directory,
                             Path.GetFileName(appPath)) ??
                         client.Configuration.MakeApplicationCustomConfiguration("consts mutation");

            var taskConnection = client.Connector.InstanceSseConnectorWithLogger();

            taskConnection.WhereClose(()=> 
                _logger.LogDebug("Connection close. Task finished.")
                );

            taskConnection.WhereSuccess(async dto =>
                await (await client.Application.DownloadApplicationAsArrayAsync(dto))
                .SaveOnAsync(save, true)
                .ContinueWith(_ => _logger.LogInformation($"La aplicación protegida ha sido guardada en {save}"))
            );

            taskConnection.WhereError(error => _logger.LogCritical(error));

            await taskConnection.ProtectSingleFileAsync(projectTest.Key,
                appBlob, config);


            System.Console.ReadKey();
        }
    }
}
