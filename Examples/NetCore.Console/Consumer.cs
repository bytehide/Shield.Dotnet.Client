using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shield.Client;
using Shield.Client.Extensions;

namespace NetCore.Console
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
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFkYTZmNDllLWVlMWYtNDhjYy05N2U0LWIxNWQyMTA2YThhNyIsInVuaXF1ZV9uYW1lIjoiOTE4ZDgxNmYtZDI4Zi00YThjLWE3MWItMzZiM2VkYTdlNjY0IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6ImQzNGY3OWQwLTY4ZGQtNDE0Zi04NmQxLWMwZTNlM2FkYWZkMSIsImV4cCI6MTYxNzExOTAxN30.JrrPC0iXYQHYFbX-WUG8eJQwm4lkfmoFr48WYIkp5EU"
                , _logger);

      

            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("pruebas de luis");

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
                .ContinueWith(_=>_logger.LogInformation($"La aplicación protegida ha sido guardada en {save}"))
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
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFkYTZmNDllLWVlMWYtNDhjYy05N2U0LWIxNWQyMTA2YThhNyIsInVuaXF1ZV9uYW1lIjoiOTE4ZDgxNmYtZDI4Zi00YThjLWE3MWItMzZiM2VkYTdlNjY0IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6ImQzNGY3OWQwLTY4ZGQtNDE0Zi04NmQxLWMwZTNlM2FkYWZkMSIsImV4cCI6MTYxNzExOTAxN30.JrrPC0iXYQHYFbX-WUG8eJQwm4lkfmoFr48WYIkp5EU"
                , _logger);

            client.Application.UploadApplicationDirectlyAsync()

            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("luis");

            var uploadApplicationDirectly = await client.Application.UploadApplicationDirectlyAsync(projectTest.Key, appPath, null);

            if (uploadApplicationDirectly.RequiresDependencies)
            {
                uploadApplicationDirectly.RequiredDependencies.ForEach(dependency =>
                    _logger.LogCritical($"Dependency ${dependency} is required."));
                throw new Exception("Some dependencies are required.");
            }

            if(string.IsNullOrEmpty(uploadApplicationDirectly.ApplicationBlob))
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
    }
}
