using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using Shield.Client.Fr.Extensions;
using Shield.Client.Fr.Helpers;
using Shield.Client.Fr.Models;
using Shield.Client.Fr.Models.API.Application;

namespace Shield.Client.Fr
{
    public class ShieldConnector
    {
        private readonly RestClient _client;
        public ShieldClient Parent { get; set; }

        public ShieldConnector(IRestClient client, ShieldClient parent)
        {
            _client = new RestClient(client.BaseUrl ?? throw new InvalidOperationException()) {Authenticator = client.Authenticator};
            //Not required version for logger (Only in dev).
            if (!client.BaseUrl.ToString().ToLower().StartsWith("https://api.bytehide.com"))
                _client.BaseUrl = new Uri(_client.BaseUrl?.AbsoluteUri.Replace(_client.BaseUrl.PathAndQuery,null) ?? throw new InvalidOperationException());
            Parent = parent;
        }
        public static ShieldConnector CreateInstance(RestClient client)
        {
            return new ShieldConnector(client, null);
        }
        public static ShieldConnector CreateInstance(RestClient client, ShieldClient parent)
        {
            return new ShieldConnector(client, parent);
        }
        public QueueConnectionExternalModel CreateQueueConnection()
        {
            return CreateQueueConnection(Guid.NewGuid().ToString());
        }
        public QueueConnectionExternalModel CreateQueueConnection(string taskId)
        {
            return new QueueConnectionExternalModel { TaskId = taskId, OnLogger = Guid.NewGuid().ToString() };
        }

        public HubConnectionExternalModel CreateHubConnection()
        {
            return CreateHubConnection(Guid.NewGuid().ToString());
        }
        public HubConnectionExternalModel CreateHubConnection(string taskId)
        {
            return new HubConnectionExternalModel { TaskId = taskId, OnLogger = Guid.NewGuid().ToString()};
        }
        private void OnCustomLog(string date, string message, string level)
        {
            DateTime.TryParse(date, out var dateTime);

            Enum.TryParse<LogLevel>(level, out var logLevel);

            Parent.CustomLogger.Log(logLevel,$"{message} at {dateTime}");
        }

        /// <summary>
        /// Build a connection to the shield bus for the current protection process.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public QueueConnection InstanceQueueConnector(QueueConnectionExternalModel externalConnection)
            => InstanceQueueConnector(externalConnection, false);
        /// <summary>
        /// Build a connection to the shield bus for the current protection process with current logger.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public QueueConnection InstanceQueueConnectorWithLogger(QueueConnectionExternalModel externalConnection)
            => InstanceQueueConnector(externalConnection, true);

        
        /// <summary>
        /// Build a connection to the shield queues service bus for the current protection process.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <param name="withLogger"></param>
        /// <returns></returns>
        private QueueConnection InstanceQueueConnector(QueueConnectionExternalModel externalConnection, bool withLogger)
        {
            try
            {
                var connection = new QueueConnection(externalConnection.TaskId);

                if (!withLogger) return connection;

                if (Parent.CustomLogger is null)
                    throw new Exception("The \"WithLogger\" method cannot be called if a custom logger has not been provided in the shield client.");

                Parent.CustomLogger?.LogDebug("The current logger has been configured as the output of the connection logs.");

                connection.On(externalConnection.OnLogger, (message, level, time) => OnCustomLog(time.ToString(CultureInfo.InvariantCulture),message,level));

                return connection;

            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical("An error occurred while creating the connection to the shield server for the current process.");
                throw new Exception($"An error occurred while creating the connection to the shield server for the current process: {ex.Message}");
            }
        }
    }
}
