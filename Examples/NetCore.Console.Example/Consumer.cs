using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bytehide.Shield.Client;
using Bytehide.Shield.Client.Extensions;
using Bytehide.Shield.Client.Models.API;

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
                @"C:\Users\juan\source\repos\ExceptionsApps\ExceptionsApps\bin\Debug\net7.0";

            var appPath = $"{directory}\\FlappyBit Modder.exe";

            var save = $"{directory}\\FlappyBit Modder_protegido.dll";

            var dependencies = Directory.GetFiles($"{directory}").ToList();

            var client = ShieldClient.CreateInstance(
                "9K5elBOWEfe35gPB7Lg3IjKtHBrzYFwFzepo92ZJ"
                , _logger);

            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("arg3");

            var uploadApplicationDirectly =
                await client.Application.UploadApplicationDirectlyAsync(projectTest.Key, appPath, null);

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

        internal async Task RunWithSse()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            const string directory =
                @"C:\Users\juan\source\repos\ExceptionsApps\ExceptionsApps\bin\Debug\net7.0";

            var appPath = $"{directory}\\FlappyBit Modder.exe";

            var save = $"{directory}\\FlappyBit Modder_protegido.dll";

            var dependencies = Directory.GetFiles($"{directory}").ToList();

            var client = ShieldClient.CreateInstance(
                "jPCY46K9cgdr9hpeI2AaBpVYNNcqcFOqmavf2FWN"
                , _logger);


            var projectTest = await client.Project.FindOrCreateExternalProjectAsync("hola");

            var uploadApplicationDirectly =
                await client.Application.UploadApplicationDirectlyAsync(projectTest.Key, appPath, null);

            if (uploadApplicationDirectly.RequiresDependencies)
            {
                uploadApplicationDirectly.RequiredDependencies.ForEach(dependency =>
                    _logger.LogCritical($"Dependency ${dependency} is required."));
                throw new Exception("Some dependencies are required.");
            }

            if (string.IsNullOrEmpty(uploadApplicationDirectly.ApplicationBlob))
                throw new Exception("Unknown exception uploading application.");

            var appBlob = uploadApplicationDirectly.ApplicationBlob;


            var config = new ProtectionConfigurationDTO
            {
                ConfigurationType = ConfigurationType.Application,
                Preset = "custom",
                Protections =
                    new System.Collections.Generic.Dictionary<string,
                        System.Collections.Generic.Dictionary<string, object>>
                    {
                        {
                            "rename",
                            new System.Collections.Generic.Dictionary<string, object> { { "rename public", true } }
                        }
                    }
            };


            //client.Configuration.FindApplicationConfiguration(
            //             directory,
            //             Path.GetFileName(appPath)) ??
            //         client.Configuration.MakeApplicationCustomConfiguration("consts mutation");

            var taskConnection = client.Connector.InstanceSseConnectorWithLogger();

            taskConnection.WhereClose(() =>
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