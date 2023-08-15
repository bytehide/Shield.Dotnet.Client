using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bytehide.Shield.Client.Extensions;
using Bytehide.Shield.Client.Helpers;
using Bytehide.Shield.Client.Models.API.Protections;
using RestSharp;

namespace Bytehide.Shield.Client
{
    public class ShieldProtections
    {
        private readonly RestClient _client;
        public ShieldClient Parent { get; set; }

        public ShieldProtections(RestClient client, ShieldClient parent)
        {
            _client = client;
            Parent = parent;
        }

        public static ShieldProtections CreateInstance(RestClient client)
        {
            return new ShieldProtections(client, null);
        }

        public static ShieldProtections CreateInstance(RestClient client, ShieldClient parent)
        {
            return new ShieldProtections(client, parent);
        }
        /// <summary>
        /// Gets available protections of a project with his key.
        /// </summary>
        /// <param name="projectKey">Project key</param>
        /// <returns></returns>
        public List<ProtectionDto> GetProtections(string projectKey)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to get project available protections.");
                LogHelper.LogDebug("Initiating the request to get project available protections.");

                var request =
                    new RestRequest("project/{projectKey}/protections/available/".ToApiRoute())
                        .AddUrlSegment("projectKey", projectKey);

                var result =  _client.Get<List<ProtectionDto>>(request);

                // Parent.CustomLogger?.LogDebug($"The available protections of {projectKey} project has been obtained correctly.");
                LogHelper.LogDebug($"The available protections of {projectKey} project has been obtained correctly.");

                return result.IsSuccessful ? result.Data : null;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while getting available protections.");
                // throw new Exception($"An error occurred while getting the available protections: {ex.Message}");

                LogHelper.LogException(ex,"An error occurred while getting available protections.");
                throw;
            }
        }
        /// <summary>
        ///  Gets available protections of a project with his key async.
        /// </summary>
        /// <param name="projectKey">Project key</param>
        /// <returns></returns>
        public async Task<List<ProtectionDto>> GetProtectionsAsync(string projectKey)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to get project available protections.");
                LogHelper.LogDebug("Initiating the request to get project available protections.");

                var request =
                    new RestRequest("project/{projectKey}/protections/available/".ToApiRoute())
                        .AddUrlSegment("projectKey", projectKey);

                var result = await _client.GetAsync<List<ProtectionDto>>(request);

                // Parent.CustomLogger?.LogDebug($"The available protections of [key]{projectKey} project has been obtained correctly.");
                LogHelper.LogDebug($"The available protections of [key]{projectKey} project has been obtained correctly.");

                return result;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while getting available protections.");
                // throw new Exception($"An error occurred while getting the available protections: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while getting available protections.");
                throw;
            }
        }
    }
}
