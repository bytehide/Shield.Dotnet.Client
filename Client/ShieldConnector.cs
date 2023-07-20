using System;
using System.Globalization;
using System.Threading.Tasks;
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
        private readonly string _bearerToken, _apiVersion, _originalBaseUrl;

        public ShieldConnector(IRestClient client, ShieldClient parent, string bearerToken, string apiVersion)
        {
            _client = new RestClient(client.BaseUrl ?? throw new InvalidOperationException())
                { Authenticator = client.Authenticator, Timeout = 1000 * 60 * 10 };
            _originalBaseUrl = client.BaseUrl.AbsoluteUri;
            //Not required version for logger (Only in dev).
            //if (!client.BaseUrl.ToString().ToLower().StartsWith("https://api.bytehide.com"))
            //    _client.BaseUrl = new Uri(_client.BaseUrl?.AbsoluteUri.Replace(_client.BaseUrl.PathAndQuery,null) ?? throw new InvalidOperationException());
            Parent = parent;
            _bearerToken = bearerToken;
            _apiVersion = apiVersion;
        }

        public static ShieldConnector CreateInstance(RestClient client, string bearerToken, string apiVersion)
        {
            return new ShieldConnector(client, null, bearerToken, apiVersion);
        }

        public static ShieldConnector CreateInstance(RestClient client, ShieldClient parent, string bearerToken,
            string apiVersion)
        {
            return new ShieldConnector(client, parent, bearerToken, apiVersion);
        }

        public HubConnectionExternalModel CreateHubConnection()
        {
            return CreateHubConnection(Guid.NewGuid().ToString());
        }

        public HubConnectionExternalModel CreateHubConnection(string taskId)
        {
            return new HubConnectionExternalModel { TaskId = taskId, OnLogger = Guid.NewGuid().ToString() };
        }

        private void OnCustomLog(string date, string message, string level)
        {
            DateTime.TryParse(date, out var dateTime);

            Enum.TryParse<LogLevel>(level, out var logLevel);

            Parent.CustomLogger.Log(logLevel, $"{message} at {dateTime}");
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
        /// Build a connection to the shield bus for the current protection process. //TODO: DOC!!
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public ServerSentEvents InstanceSseConnector()
            => InstanceSseConnector(false);

        /// <summary>
        /// Build a connection to the shield hub for the current protection process with current logger async.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public async Task<HubConnection> InstanceHubConnectorWithLoggerAsync(
            HubConnectionExternalModel externalConnection)
            => await InstanceHubConnectorAsync(externalConnection, true);

        /// <summary>
        /// Build a connection to the shield hub for the current protection process with current logger.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public HubConnection InstanceHubConnectorWithLogger(HubConnectionExternalModel externalConnection)
            => InstanceHubConnector(externalConnection, true);

        /// <summary>
        /// Build a connection to the shield bus for the current protection process with current logger. //TODO: DOC!!
        /// </summary>
        /// <returns></returns>
        public ServerSentEvents InstanceSseConnectorWithLogger()
            => InstanceSseConnector(true);

        /// <summary>
        /// Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications with current logger.
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <returns></returns>
        public bool InstanceAndStartHubConnectorWithLogger(HubConnectionExternalModel externalConnection,
            out StartedConnection started)
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
        public bool InstanceAndStartConnector(HubConnectionExternalModel externalConnection,
            out StartedConnection started)
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
        /// Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications with current logger. //TODO: DOC!!!
        /// </summary>
        /// <param name="externalConnection"></param>
        /// <param name="withLogger"></param>
        /// <returns></returns>
        private async Task<HubConnection> InstanceHubConnectorAsync(HubConnectionExternalModel externalConnection, bool withLogger)
        {
            try
            {
                LogHelper.LogDebug("Starting the negotiation with the notification server.");

                var request = new RestRequest("/logger/negotiate");

                var negotiation = await _client.GetAsync<NegotiateModel>(request);

                //Creates logger hub
                var connection = new HubConnectionBuilder()
                    .WithUrl($"{negotiation?.Url}{externalConnection.ToQueryString()}", options =>
                    {
                        options.SkipNegotiation = true; //Negotiation previously done.
                        options.Transports = HttpTransportType.WebSockets;

                        options.AccessTokenProvider =
                            () => Task.FromResult(negotiation?.AccessToken); //Add bearer token for current connection
                    })
#if !DEBUG
                    .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.None))
#endif
                    .WithAutomaticReconnect()
                    .Build(); //Build hub

                // Parent.CustomLogger?.LogDebug("The connection to the shield notification server has been created.");
                LogHelper.LogDebug("The connection to the shield notification server has been created.");

                if (!withLogger) return connection;

                // if (Parent.CustomLogger is null)
                //     throw new Exception("The \"WithLogger\" method cannot be called if a custom logger has not been provided in the shield client.");

                // Parent.CustomLogger?.LogDebug("The current logger has been configured as the output of the connection logs.");
                LogHelper.LogDebug("The current logger has been configured as the output of the connection logs.");

                connection.OnLog(externalConnection.OnLogger, OnCustomLog);

                return connection;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical("An error occurred while creating the connection to the shield server for the current process.");
                // throw new Exception($"An error occurred while creating the connection to the shield server for the current process: {ex.Message}");

                LogHelper.LogException(ex,
                    "An error occurred while creating the connection to the shield server for the current process.");
                throw;
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
                // Parent.CustomLogger?.LogDebug("Starting the negotiation with the notification server.");
                LogHelper.LogDebug("Starting the negotiation with the notification server.");

                var request = new RestRequest("/logger/negotiate");
                var negotiation = _client.Get<NegotiateModel>(request);

                if (!negotiation.IsSuccessful)
                {
                    LogHelper.LogError("Can't negotiate");
                    throw new Exception();
                }

                //Creates logger hub
                var connection = new HubConnectionBuilder()
                    .WithUrl($"{negotiation?.Data.Url}{externalConnection.ToQueryString()}", options =>
                    {
                        options.SkipNegotiation = true; //Negotiation previously done.
                        options.Transports = HttpTransportType.WebSockets;

                        options.AccessTokenProvider =
                            () => Task.FromResult(negotiation?.Data
                                .AccessToken); //Add bearer token for current connection
                    })
#if !DEBUG
                    .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.None))
#endif
                    .WithAutomaticReconnect()
                    .Build(); //Build hub

                // Parent.CustomLogger?.LogDebug("The connection to the shield notification server has been created.");
                LogHelper.LogDebug("The connection to the shield notification server has been created.");

                if (!withLogger) return connection;

                // if (Parent.CustomLogger is null)
                //     throw new Exception(
                //         "The \"WithLogger\" method cannot be called if a custom logger has not been provided in the shield client.");

                LogHelper.LogDebug( "The current logger has been configured as the output of the connection logs.");

                connection.OnLog(externalConnection.OnLogger, OnCustomLog);

                return connection;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical( "An error occurred while creating the connection to the shield server for the current process.");
                // throw new Exception( $"An error occurred while creating the connection to the shield server for the current process: {ex.Message}");

                LogHelper.LogException(ex, "An error occurred while creating the connection to the shield server for the current process.");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="withLogger"></param>
        /// <returns></returns>
        private ServerSentEvents InstanceSseConnector(bool withLogger)
        {
            try
            {
                var connection = new ServerSentEvents(_bearerToken, _apiVersion, _originalBaseUrl);

                if (!withLogger) return connection;

                // if (Parent.CustomLogger is null)
                //     throw new Exception(
                //         "The \"WithLogger\" method cannot be called if a custom logger has not been provided in the shield client.");

                // Parent.CustomLogger?.LogDebug( "The current logger has been configured as the output of the connection logs.");
                LogHelper.LogDebug( "The current logger has been configured as the output of the connection logs.");

                connection.SetDefaultLogger((message, level, time) => OnCustomLog(time.ToString(CultureInfo.InvariantCulture), message, level));

                return connection;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical( "An error occurred while creating the connection to the shield server for the current process.");
                // throw new Exception( $"An error occurred while creating the connection to the shield server for the current process: {ex.Message}");

                LogHelper.LogException( ex, "An error occurred while creating the connection to the shield server for the current process.");
                throw;
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
        public void On(string method, Action<string, string> action) => Connection.On(method, action);
        public void On(string method, Action<string, string, string> action) => Connection.On(method, action);
    }
}