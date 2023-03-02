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
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5NDNmMGMwMS0yOTBiLTQ4NjYtYTAyZi1lN2Q4NTU2OWE5OTQiLCJqdGkiOiIxMGU5MzU5NDBhOGFjOGI4MDQyMzVkYzI0ZmY2NmJkMmEwODZmMmNhNmI0NWFhNDkwMzQ5YTNiZDcyMzQzZmRiM2RiZmU3Zjk0ODFkMGJlMCIsImlhdCI6MTYzMzQzMzE0My43Mjk3NTUsIm5iZiI6MTYzMzQzMzE0My43Mjk3NTksImV4cCI6MTY2NDk2OTE0My42ODg1MSwic3ViIjoiOTQ1OTc5NTgtY2JjYi00OThhLTllNTAtYzQ0NmUzYTIyZDkyIiwic2NvcGVzIjpbXX0.a7MTfyKDV6TrLg9r-fpn0qTazcWGzBM1uzZiOjB2YT-ex_fEIZlWs0gO7TbTVQbs3E9LuLO-HNQz5Q0VQeN9gW1tfW-aB5mIN0T2FNMeECcqT2UQeVD5W1AGU3K4oadOMUMif9Hqujgyi3mXU4zt_EubHZ-A8eLhUDcbP5UdLJE7FeYnyCHLXM4RbWgzugqRMmwzt6qctQ_uPXLfdiSL6Br0Cb5mA45tBwU2hcWfN3wLeCIeIncN1HxOpBBphMjOLP27N39OMf0uFJ4eqvnWC7AM_aEsWGCuse9svxRYSeznFVoU9KaPu_X9geRbBBat35QnDjnzJyHOa30EiK-qnuRlrAX-Qhrm1LTGx0hQY461Yx7hv2tnlT-s2_s41w02DSAlxQsNKrxAH5U1OKUujgLEJNsEONRnD7oYBuu2NisRFg8RWfJi3S6kZS8UjrXR646J7AkBB8R2EUeJULEv1SUoDLTveCl1NyDJQjvhFroQrhKsuT4QTH2Gd8kWcjEfRrm63qjzFMLGDo7JcMCh_O4lXNXdJ6Bk8T3eWdCk1vfUoPeiZ9ppuYtR4XncuNk-93sb3FchU-dNZGlcrq4LEcyn1lNMflCSARfqdbvdwn-4QYu7X03nfZqmQ2mzxmr7aUOu1z2idpvACKKaPJ-G9akWOzfVYPKgH0WA0XH81zE"
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

        internal async Task RunWithSse()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            var client = ShieldClient.CreateInstance(
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5NDNmMGMwMS0yOTBiLTQ4NjYtYTAyZi1lN2Q4NTU2OWE5OTQiLCJqdGkiOiIxMGU5MzU5NDBhOGFjOGI4MDQyMzVkYzI0ZmY2NmJkMmEwODZmMmNhNmI0NWFhNDkwMzQ5YTNiZDcyMzQzZmRiM2RiZmU3Zjk0ODFkMGJlMCIsImlhdCI6MTYzMzQzMzE0My43Mjk3NTUsIm5iZiI6MTYzMzQzMzE0My43Mjk3NTksImV4cCI6MTY2NDk2OTE0My42ODg1MSwic3ViIjoiOTQ1OTc5NTgtY2JjYi00OThhLTllNTAtYzQ0NmUzYTIyZDkyIiwic2NvcGVzIjpbXX0.a7MTfyKDV6TrLg9r-fpn0qTazcWGzBM1uzZiOjB2YT-ex_fEIZlWs0gO7TbTVQbs3E9LuLO-HNQz5Q0VQeN9gW1tfW-aB5mIN0T2FNMeECcqT2UQeVD5W1AGU3K4oadOMUMif9Hqujgyi3mXU4zt_EubHZ-A8eLhUDcbP5UdLJE7FeYnyCHLXM4RbWgzugqRMmwzt6qctQ_uPXLfdiSL6Br0Cb5mA45tBwU2hcWfN3wLeCIeIncN1HxOpBBphMjOLP27N39OMf0uFJ4eqvnWC7AM_aEsWGCuse9svxRYSeznFVoU9KaPu_X9geRbBBat35QnDjnzJyHOa30EiK-qnuRlrAX-Qhrm1LTGx0hQY461Yx7hv2tnlT-s2_s41w02DSAlxQsNKrxAH5U1OKUujgLEJNsEONRnD7oYBuu2NisRFg8RWfJi3S6kZS8UjrXR646J7AkBB8R2EUeJULEv1SUoDLTveCl1NyDJQjvhFroQrhKsuT4QTH2Gd8kWcjEfRrm63qjzFMLGDo7JcMCh_O4lXNXdJ6Bk8T3eWdCk1vfUoPeiZ9ppuYtR4XncuNk-93sb3FchU-dNZGlcrq4LEcyn1lNMflCSARfqdbvdwn-4QYu7X03nfZqmQ2mzxmr7aUOu1z2idpvACKKaPJ-G9akWOzfVYPKgH0WA0XH81zE"
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
