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

            const string directory =
                @"C:\Users\juana\source\repos\Ornitorrinco\Ornitorrinco\bin\Debug\netcoreapp3.1\";

            var appPath = $"{directory}Ornitorrinco.dll";

            var save = $"{directory}Ornitorrinco_protegida.dll";

            var dependencies = Directory.GetFiles($"{directory}").ToList();

            var client = ShieldClient.CreateInstance(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE4ODgzMmEyLTUxODktNDMwZS05NGFmLTc3MTJkZTBiM2FmZCIsInVuaXF1ZV9uYW1lIjoiOTE4ZDgxNmYtZDI4Zi00YThjLWE3MWItMzZiM2VkYTdlNjY4IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6ImUwZDYzYWZmLTliZmUtNDlmZi1iMmNjLTUwMTU0YjY5MTc0NSIsImV4cCI6MTYyNTE2ODcxM30.0TdYvWiAJDSok_SBuWt8vX1fGfmjRLg9K9FrtPbgAw8"
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


            var config = client.Configuration.FindApplicationConfiguration(
                             directory,
                             Path.GetFileName(appPath)) ??
                         client.Configuration.MakeApplicationCustomConfiguration("consts mutation");

            var taskConnection = client.Connector.InstanceSseConnectorWithLogger();

            taskConnection.OnClose(_ => 
                _logger.LogDebug("Connection close. Task finished.")
                );

            taskConnection.OnSuccess(async dto =>
                await (await client.Application.DownloadApplicationAsArrayAsync(dto))
                .SaveOnAsync(save, true)
                .ContinueWith(_ => _logger.LogInformation($"La aplicación protegida ha sido guardada en {save}"))
            );

            taskConnection.OnError(error => _logger.LogCritical(error));

            await taskConnection.ProtectSingleFileAsync(projectTest.Key,
                appBlob, config);


            System.Console.ReadKey();
        }
    }
}
