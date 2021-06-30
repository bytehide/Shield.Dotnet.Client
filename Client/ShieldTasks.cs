using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;

namespace Shield.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class ShieldTasks
    {
        private readonly RestClient _client;
        public ShieldClient Parent { get; set; }

        public ShieldTasks(RestClient client, ShieldClient parent)
        {
            _client = client;
            Parent = parent;
        }
        public static ShieldTasks CreateInstance(RestClient client)
        {
            return new ShieldTasks(client, null);
        }
        public static ShieldTasks CreateInstance(RestClient client, ShieldClient parent)
        {
            return new ShieldTasks(client, parent);
        }

        public async Task<ProtectionResult> ProtectSingleFileAsync(string projectKey, string fileBlob,
            HubConnectionExternalModel hubConnection, ApplicationConfigurationDto configuration)
            => await ProtectSingleFileAsync(projectKey, fileBlob, hubConnection.TaskId, configuration);

        public async Task<ProtectionResult> ProtectSingleFileAsync(string projectKey, string fileBlob,
            QueueConnectionExternalModel queueConnection, ApplicationConfigurationDto configuration, string queueMethod)
            => await ProtectSingleFileAsync(projectKey, fileBlob, queueConnection.TaskId, configuration, queueMethod);

        public ProtectionResult ProtectSingleFile(string projectKey, string fileBlob,
            HubConnectionExternalModel hubConnection, ApplicationConfigurationDto configuration)
            => ProtectSingleFile(projectKey, fileBlob, hubConnection.TaskId, configuration);

        public ProtectionResult ProtectSingleFile(string projectKey, string fileBlob,
            QueueConnectionExternalModel queueConnection, ApplicationConfigurationDto configuration, string queueMethod)
            => ProtectSingleFile(projectKey, fileBlob, queueConnection.TaskId, configuration, queueMethod);

        public async Task<ProtectionResult> ProtectSingleFileAsync(string projectKey, string fileBlob, string runKey, ApplicationConfigurationDto configuration, string queueMethod = null, string sseMethod = null)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Initiating the request to protect a single file.");

                var request =
                    new RestRequest("/protection/protect/single")
                        .AddQueryParameter("projectKey", projectKey)
                        .AddQueryParameter("fileBlob", fileBlob)
                        .AddQueryParameter("runKey", runKey)
                        .AddJsonBody(configuration ?? new ApplicationConfigurationDto { InheritFromProject = true });

                if (!string.IsNullOrEmpty(queueMethod))
                {
                    request.AddQueryParameter("useQueues", "true");
                    request.AddQueryParameter("onLoggerQueue", queueMethod);
                }

                if (!string.IsNullOrEmpty(sseMethod))
                {
                    request.AddQueryParameter("useSse", "true");
                    request.AddQueryParameter("onLoggerSse", sseMethod);
                }



                //var result = await _client.PostAsync<ProtectionResult>(request);

                _client.ThrowOnDeserializationError = true;

                try
                {
                    var result = await _client.PostAsync<ProtectionResult>(request);

                    Parent.CustomLogger?.LogDebug($"The protection process has started successfully with the identifier: {runKey}");

                    return result;

                }
                catch (DeserializationException ex)
                {
                    throw new Exception(ex.Response.Content, ex);
                }
            }
            catch (Exception ex) when (ex.InnerException.GetType() == typeof(DeserializationException))
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while starting the protection process.");
                throw ex;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while starting the protection process.");
                throw new Exception($"An error occurred while starting the protection process. {ex.Message}");
            }
        }





        public ProtectionResult ProtectSingleFile(string projectKey, string fileBlob, string runKey, ApplicationConfigurationDto configuration, string queueMethod = null, string sseMethod = null)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Initiating the request to protect a single file.");

                var request =
                    new RestRequest("/protection/protect/single")
                        .AddQueryParameter("projectKey", projectKey)
                        .AddQueryParameter("fileBlob", fileBlob)
                        .AddQueryParameter("runKey", runKey)
                        .AddJsonBody(configuration ?? new ApplicationConfigurationDto { InheritFromProject = true });

                if (!string.IsNullOrEmpty(queueMethod))
                {
                    request.AddQueryParameter("useQueues", "true");
                    request.AddQueryParameter("onLoggerQueue", queueMethod);
                }

                if (!string.IsNullOrEmpty(sseMethod))
                {
                    request.AddQueryParameter("useSse", "true");
                    request.AddQueryParameter("onLoggerSse", sseMethod);
                }


                var result = _client.Post<ProtectionResult>(request);

                if (!result.IsSuccessful)
                    return null;

                Parent.CustomLogger?.LogDebug($"The protection process has started successfully with the identifier: {runKey}");

                return result.Data;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while starting the protection process.");
                throw new Exception($"An error occurred while starting the protection process. {ex.Message}");
            }
        }

    }
}
