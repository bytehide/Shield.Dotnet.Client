using System;
using System.Globalization;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using RestSharp;
using Shield.Client.Extensions;
using Shield.Client.Helpers;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;

namespace Shield.Client
{
    public class ShieldConnector
    {
        private readonly RestClient _client;
        public ShieldClient Parent { get; set; }

        public ShieldConnector(IRestClient client, ShieldClient parent)
        {
            _client = new RestClient(client.BaseUrl ?? throw new InvalidOperationException()) {Authenticator = client.Authenticator};
           //Not required version for logger.
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
        /// Build a connection to the shield hub for the current protection process async.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public async Task<HubConnection> InstanceHubConnectorAsync(HubConnectionExternalModel externalConnection)
            => await InstanceHubConnectorAsync(externalConnection, false);
        /// <summary>
        /// Build a connection to the shield hub for the current protection process.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public HubConnection InstanceHubConnector(HubConnectionExternalModel externalConnection)
            => InstanceHubConnector(externalConnection, false);
        /// <summary>
        /// Build a connection to the shield bus for the current protection process.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public QueueConnection InstanceQueueConnector(QueueConnectionExternalModel externalConnection)
            => InstanceQueueConnector(externalConnection, false);
        /// <summary>
        /// Build a connection to the shield hub for the current protection process with current logger async.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public async Task<HubConnection> InstanceHubConnectorWithLoggerAsync(HubConnectionExternalModel externalConnection)
            => await InstanceHubConnectorAsync(externalConnection, true);
        /// <summary>
        /// Build a connection to the shield hub for the current protection process with current logger.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public HubConnection InstanceHubConnectorWithLogger(HubConnectionExternalModel externalConnection)
            => InstanceHubConnector(externalConnection, true);
        /// <summary>
        /// Build a connection to the shield bus for the current protection process with current logger.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public QueueConnection InstanceQueueConnectorWithLogger(QueueConnectionExternalModel externalConnection)
            => InstanceQueueConnector(externalConnection, true);
        /// <summary>
        /// Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications with current logger.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public bool InstanceAndStartHubConnectorWithLogger(HubConnectionExternalModel externalConnection, out StartedConnection started)
        {
            try
            {
                started = Task.Run(async () =>
                {
                    var instanceConnectorAsync = await InstanceHubConnectorWithLoggerAsync(externalConnection);
                    await instanceConnectorAsync.StartAsync();
                    return new StartedConnection(instanceConnectorAsync);
                }).Result;

                return true;
            }
            catch
            {
                started = null;
                return false;
            }
        }
        /// <summary>
        /// Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <param name="started"></param>
        /// <returns></returns>
        public bool InstanceAndStartConnector(HubConnectionExternalModel externalConnection, out StartedConnection started)
        {
            try
            {
                started = Task.Run(async () =>
                {
                    var instanceConnectorAsync = await InstanceHubConnectorAsync(externalConnection);
                    await instanceConnectorAsync.StartAsync();
                    return new StartedConnection(instanceConnectorAsync);
                }).Result;

                return true;
            }
            catch
            {
                started = null;
                return false;
            }
        }
        /// <summary>
        /// Build a connection to the shield hub for the current protection process async.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <param name="withLogger"></param>
        /// <returns></returns>
        private async Task<HubConnection> InstanceHubConnectorAsync(HubConnectionExternalModel externalConnection, bool withLogger)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Starting the negotiation with the notification server.");

                var request = new RestRequest("/logger/negotiate");

                var negotiation = await _client.GetAsync<NegotiateModel>(request);

                //Creates logger hub
                var connection = new HubConnectionBuilder()
                    .WithUrl($"{negotiation?.Url}{externalConnection.ToQueryString()}", options =>
                    {
                        options.SkipNegotiation = true; //Negotiation previously done.
                        options.Transports = HttpTransportType.WebSockets;

                        options.AccessTokenProvider = () => Task.FromResult(negotiation?.AccessToken); //Add bearer token for current connection
                    })
#if !DEBUG
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.None))
#endif
                    .WithAutomaticReconnect()
                    .Build(); //Build hub

                Parent.CustomLogger?.LogDebug("The connection to the shield notification server has been created.");

                if (!withLogger) return connection;

                if (Parent.CustomLogger is null)
                    throw new Exception("The \"WithLogger\" method cannot be called if a custom logger has not been provided in the shield client.");

                Parent.CustomLogger?.LogDebug("The current logger has been configured as the output of the connection logs.");

                connection.OnLog(externalConnection.OnLogger, OnCustomLog);

                return connection;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical("An error occurred while creating the connection to the shield server for the current process.");
                throw new Exception($"An error occurred while creating the connection to the shield server for the current process: {ex.Message}");
            }
        }
        /// <summary>
        /// Build a connection to the shield hub for the current protection process.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <param name="withLogger"></param>
        /// <returns></returns>
        private HubConnection InstanceHubConnector(HubConnectionExternalModel externalConnection, bool withLogger)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Starting the negotiation with the notification server.");

                var request = new RestRequest("/logger/negotiate");

                var negotiation = _client.Get<NegotiateModel>(request);

                if (!negotiation.IsSuccessful)
                    throw new Exception("Can't negotiate");

                //Creates logger hub
                var connection = new HubConnectionBuilder()
                    .WithUrl($"{negotiation?.Data.Url}{externalConnection.ToQueryString()}", options =>
                    {
                        options.SkipNegotiation = true; //Negotiation previously done.
                        options.Transports = HttpTransportType.WebSockets;

                        options.AccessTokenProvider = () => Task.FromResult(negotiation?.Data.AccessToken); //Add bearer token for current connection
                    })
#if !DEBUG
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.None))
#endif
                    .WithAutomaticReconnect()
                    .Build(); //Build hub

                Parent.CustomLogger?.LogDebug("The connection to the shield notification server has been created.");

                if (!withLogger) return connection;

                if (Parent.CustomLogger is null)
                    throw new Exception("The \"WithLogger\" method cannot be called if a custom logger has not been provided in the shield client.");

                Parent.CustomLogger?.LogDebug("The current logger has been configured as the output of the connection logs.");

                connection.OnLog(externalConnection.OnLogger, OnCustomLog);

                return connection;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical("An error occurred while creating the connection to the shield server for the current process.");
                throw new Exception($"An error occurred while creating the connection to the shield server for the current process: {ex.Message}");
            }
        }
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
    /// <summary>
    /// This class is an adaptation of a SignalR HubConnection to handle Shield processes from .net framework applications in a comfortable way.
    /// </summary>
    public class StartedConnection
    {
        private HubConnection Connection { get; }
        public StartedConnection(HubConnection connection) => Connection = connection;
        public void On(string method, Action<ProtectedApplicationDto> action) => Connection.On(method, action);
        public void On(string method, Action action) => Connection.On(method, action);
        public void On(string method, Action<string> action) => Connection.On(method, action);
        public void On(string method, Action<string,string> action) => Connection.On(method, action);
        public void On(string method, Action<string,string,string> action) => Connection.On(method, action);
    }
}
