using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Shield.Client.Fr.Models;

namespace Shield.Client.Fr.Extensions
{
    public class QueueConnection
    {
        private ServiceBusClient BusClient { get; }
        private ServiceBusProcessor BusProcessor { get; }
        private Dictionary<string, Action<string, string, DateTime>> Methods { get; } = new Dictionary<string, Action<string, string, DateTime>>();

        public void On(string method, Action<string, string, DateTime> action) =>
            Methods.Add(method, action);

        public void Destroy(string method) => Methods.Remove(method);

        public async Task StartAsync()
        {
            await BusProcessor.StartProcessingAsync();
        }
        public async Task StopAsync()
        {
            await BusProcessor.StartProcessingAsync();
        }
        public QueueConnection(string queueName)
        {
            //TODO: Change
            BusClient = new ServiceBusClient("Endpoint=sb://dotnetsafer.servicebus.windows.net/;SharedAccessKeyName=CLI;SharedAccessKey=koL+6ZQLUUVo7du5dtaFuo8cYrzDAIhXpHDWBQ1vHnU=");

            BusProcessor = BusClient.CreateProcessor(queueName);
            BusProcessor.ProcessMessageAsync += ReceiverOnProcessMessageAsync;
            BusProcessor.ProcessErrorAsync += ReceiverOnProcessErrorAsync;
        }

        private async Task ReceiverOnProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = JsonSerializer.Deserialize<QueueLogMessageModel>(arg.Message.Body.ToString());

            await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(false);

            if (message is null)
                return;

            if (!Methods.TryGetValue(message.Method, out var action))
                return;

            action(message.Message,message.LogLevelString,message.Time);

        }
        private static Task ReceiverOnProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            return null;
        }
    }
}
