using System;
using System.Threading.Tasks;
using Bytehide.Shield.Client.Extensions;
using Bytehide.Shield.Client.Helpers;
using Bytehide.Shield.Client.Models.API.Project;
using RestSharp;

namespace Bytehide.Shield.Client
{
    public class ShieldProject
    {
        private readonly RestClient _client;

        public ShieldClient Parent { get; set; }

        public ShieldProject(RestClient client, ShieldClient parent)
        {
            _client = client;
            Parent = parent;
        }

        public static ShieldProject CreateInstance(RestClient client)
        {
            return new ShieldProject(client, null);
        }

        public static ShieldProject CreateInstance(RestClient client, ShieldClient parent)
        {
            return new ShieldProject(client, parent);
        }

        /// <summary>
        /// Find or creates a Shield project
        /// </summary>
        /// <param name="projectName">Project name, used for find or as new project name</param>
        /// <returns></returns>
        public async Task<ProjectDto> FindOrCreateExternalProjectAsync(string projectName)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to find or create external project.");
                LogHelper.LogDebug("Initiating the request to find or create external project.");

                var request =
                    new RestRequest("project/externalProject/{projectName}".ToApiRoute())
                        .AddUrlSegment("projectName", projectName);

                var result = await _client.GetAsync<ProjectDto>(request);

                // Parent.CustomLogger?.LogDebug($"The {projectName} project has been successfully obtained.");
                LogHelper.LogDebug($"The {projectName} project has been successfully obtained.");

                return result;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while searching or trying to create the {projectName} project.");
                // throw new Exception($"An error occurred while searching or trying to create the {projectName} project: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while searching or trying to create the {projectName} project");
                throw new Exception();
            }
        }

        /// <summary>
        /// Find or creates a Shield project
        /// </summary>
        /// <param name="projectName">Project name, used for find or as new project name</param>
        /// <returns></returns>
        public ProjectDto FindOrCreateExternalProject(string projectName)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to find or create external project.");
                LogHelper.LogDebug("Initiating the request to find or create external project.");

                var request =
                    new RestRequest("/project/externalProject/{projectName}".ToApiRoute())
                        .AddUrlSegment("projectName", projectName);

                var result = _client.Get<ProjectDto>(request);

                if (!result.IsSuccessful)
                    return null;

                // Parent.CustomLogger?.LogDebug($"The {projectName} project has been successfully obtained.");
                LogHelper.LogDebug($"The {projectName} project has been successfully obtained.");

                return result.Data;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical( $"An error occurred while searching or trying to create the {projectName} project.");
                // throw new Exception( $"An error occurred while searching or trying to create the {projectName} project: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while searching or trying to create the {projectName} project.");
                throw new Exception();
            }
        }

        /// <summary>
        /// Find by Id or creates a Shield project
        /// </summary>
        /// <param name="projectName">Project name, used only when creates new project.</param>
        /// <param name="projectKey">Project id to find</param>
        /// <returns></returns>
        public async Task<ProjectDto> FindByIdOrCreateExternalProjectAsync(string projectName, string projectKey)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to find or create external project.");
                LogHelper.LogDebug("Initiating the request to find or create external project.");

                var request =
                    new RestRequest("/project/externalProject/{projectName}".ToApiRoute())
                        .AddUrlSegment("projectName", projectName)
                        .AddQueryParameter("projectKey", projectKey);

                var result = await _client.GetAsync<ProjectDto>(request);

                // Parent.CustomLogger?.LogDebug($"The {projectName} project has been successfully obtained.");
                LogHelper.LogDebug($"The {projectName} project has been successfully obtained.");

                return result;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical( $"An error occurred while searching or trying to create the {projectName} project.");
                // throw new Exception( $"An error occurred while searching or trying to create the {projectName} project: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while searching or trying to create the {projectName} project.");
                throw new Exception();
            }
        }

        /// <summary>
        /// Find by Id or creates a Shield project
        /// </summary>
        /// <param name="projectName">Project name, used only when creates new project.</param>
        /// <param name="projectKey">Project id to find</param>
        /// <returns></returns>
        public ProjectDto FindByIdOrCreateExternalProject(string projectName, string projectKey)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to find or create external project.");
                LogHelper.LogDebug("Initiating the request to find or create external project.");

                var request =
                    new RestRequest("/project/externalProject/{projectName}".ToApiRoute())
                        .AddUrlSegment("projectName", projectName)
                        .AddQueryParameter("projectKey", projectKey);

                var result = _client.Get<ProjectDto>(request);

                if (!result.IsSuccessful) return null;

                // Parent.CustomLogger?.LogDebug($"The {projectName} project has been successfully obtained.");
                LogHelper.LogDebug($"The {projectName} project has been successfully obtained.");

                return result.Data;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical( $"An error occurred while searching or trying to create the {projectName} project.");
                // throw new Exception( $"An error occurred while searching or trying to create the {projectName} project: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while searching or trying to create the {projectName} project.");
                throw new Exception();
            }
        }
    }
}