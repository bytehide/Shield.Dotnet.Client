using System;
using System.Threading.Tasks;
using RestSharp;
using Shield.Client.Extensions;
using Shield.Client.Helpers;
using Shield.Client.Models;
using Shield.Client.Models.API;

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
            HubConnectionExternalModel hubConnection, ProtectionConfigurationDTO configuration)
            => await ProtectSingleFileWithSignalRAsync(projectKey, fileBlob, hubConnection.TaskId, configuration);

        public ProtectionResult ProtectSingleFile(string projectKey, string fileBlob,
            HubConnectionExternalModel hubConnection, ProtectionConfigurationDTO configuration)
            => ProtectSingleFileWithSignalR(projectKey, fileBlob, hubConnection.TaskId, configuration);

        public async Task<ProtectionResult> ProtectSingleFileWithSignalRAsync(string projectKey, string fileBlob, string runKey, ProtectionConfigurationDTO configuration)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to protect a single file.");
                LogHelper.LogDebug("Initiating the request to protect a single file.");

                var request =
                    new RestRequest("/protection/protect/single".ToApiRoute())
                        .AddQueryParameter("projectKey", projectKey)
                        .AddQueryParameter("fileBlob", fileBlob)
                        .AddQueryParameter("runKey", runKey)
                        .AddJsonBody(configuration.Serialize() ?? Parent.Configuration.Default().Inherit().Serialize());

                _client.ThrowOnDeserializationError = true;

                try
                {
                    var result = await _client.PostAsync<ProtectionResult>(request);

                    // Parent.CustomLogger?.LogDebug($"The protection process has started successfully with the identifier: {runKey}");
                    LogHelper.LogDebug($"The protection process has started successfully with the identifier: {runKey}");

                    return result;

                }
                catch (DeserializationException ex)
                {
                    LogHelper.LogException(ex);
                    throw new Exception(ex.Response.Content, ex);
                }
            }
            catch (Exception ex) when (ex.InnerException != null && ex.InnerException.GetType() == typeof(DeserializationException))
            {
                // Parent.CustomLogger?.LogCritical("An error occurred while starting the protection process.");
                LogHelper.LogException(ex, "An error occurred while starting the protection process.");
                throw;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical("An error occurred while starting the protection process.");
                // throw new Exception($"An error occurred while starting the protection process. {ex.Message}");
                
                LogHelper.LogException(ex,"An error occurred while starting the protection process.");
                throw;
            }
        }


        public ProtectionResult ProtectSingleFileWithSignalR(string projectKey, string fileBlob, string runKey, ProtectionConfigurationDTO configuration)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to protect a single file.");
                LogHelper.LogDebug("Initiating the request to protect a single file.");

                var request =
                    new RestRequest("/protection/protect/single".ToApiRoute())
                        .AddQueryParameter("projectKey", projectKey)
                        .AddQueryParameter("fileBlob", fileBlob)
                        .AddQueryParameter("runKey", runKey)
                        .AddJsonBody(configuration.Serialize() ?? Parent.Configuration.Default().Inherit().Serialize());

                var result = _client.Post<ProtectionResult>(request);

                if (!result.IsSuccessful)
                    return null;

                // Parent.CustomLogger?.LogDebug($"The protection process has started successfully with the identifier: {runKey}");
                LogHelper.LogDebug($"The protection process has started successfully with the identifier: {runKey}");

                return result.Data;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while starting the protection process.");
                // throw new Exception($"An error occurred while starting the protection process. {ex.Message}");

                LogHelper.LogException(ex,$"An error occurred while starting the protection process.");
                throw;
            }
        }

    }
}
