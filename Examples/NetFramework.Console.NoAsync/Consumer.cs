using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Shield.Client;
using Shield.Client.Extensions;

namespace NetFramework.Console.NoAsync
{
    public class Consumer
    {
        private readonly ILogger _logger;

        public Consumer(ILogger<Consumer> logger)
        {
            _logger = logger;

        }

        internal void Run()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            const string directory =
                "C:\\Users\\juana\\source\\repos\\Shield.VSIX\\src\\ApiConsumer\\bin\\Debug\\net5.0\\";

            var dependencies = Directory.GetFiles($"{directory}deps").ToList();

            var appPath = $"{directory}ApiConsumer2.dll";

            var save = $"{directory}ApiConsumer3.dll";

            var client = ShieldClient.CreateInstance(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE4ODgzMmEyLTUxODktNDMwZS05NGFmLTc3MTJkZTBiM2FmZCIsInVuaXF1ZV9uYW1lIjoiOTE4ZDgxNmYtZDI4Zi00YThjLWE3MWItMzZiM2VkYTdlNjY4IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6IjdlZGM4OTA5LTlmNTAtNGNiZi1hMzQyLTQ3MWEwY2E1ZjdiNCIsImV4cCI6MTYxMzQxOTc5MX0._PhbQxQEihsNLjPHCmXbaEVSwzwnQ7nUBb0zvsACA-8",
                _logger
            );

            var projectTest = client.Project.FindOrCreateExternalProject("testing");

            var uploadApplicationDirectly =
                client.Application.UploadApplicationDirectly(projectTest.Key, appPath, dependencies);

            if (uploadApplicationDirectly.RequiresDependencies ||
                string.IsNullOrEmpty(uploadApplicationDirectly.ApplicationBlob))
                throw new Exception("Se necesitan las dependencias.");

            var appBlob = uploadApplicationDirectly.ApplicationBlob;

            var connection = client.Connector.CreateHubConnection();

            //Here is the difference between .NET Core and .NET Framework:
            if (!client.Connector.InstanceAndStartHubConnectorWithLogger(connection, out var taskConnection))
                throw new Exception("No se ha podido conectar con el servicio de SignalR");

            //Not required, because we are calling InstanceAndStartConnectorWithLogger and not InstanceAndStartConnector:

            //taskConnection.OnLog(connection.OnLogger, (date, message, level) =>
            //{
            //    DateTime.TryParse(date, out var dateTime);

            //    System.Console.WriteLine($"[{level}]: {message}");
            //});

            var config = client.Configuration.FindApplicationConfiguration(directory,
                             Path.GetFileName(appPath)) ??
                         client.Configuration.MakeApplicationCustomConfiguration("consts mutation");

            var protect = client.Tasks.ProtectSingleFile(
                projectTest.Key,
                appBlob,
                connection,
                config);

            protect.OnError(taskConnection, error => _logger.LogError(error));

            protect.OnClose(taskConnection, _ => _logger.LogInformation("Connection close. Task finished."));

            protect.OnSuccess(taskConnection, async dto =>
                await (await client.Application.DownloadApplicationAsArrayAsync(dto))
                    .SaveOnAsync(save, true)
                    .ContinueWith(_ => _logger.LogInformation($"La aplicación protegida ha sido guardada en {save}."))
            );

            System.Console.ReadKey();
        }
    }
}
