using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bytehide.Shield.Client.Extensions;
using Bytehide.Shield.Client.Helpers;
using Bytehide.Shield.Client.Models;
using Bytehide.Shield.Client.Models.API.Application;
using RestSharp;

namespace Bytehide.Shield.Client
{
    public class ShieldApplication
    {
        private readonly RestClient _client;
        public ShieldClient Parent { get; set; }

        public ShieldApplication(RestClient client, ShieldClient parent)
        {
            _client = client;
            Parent = parent;
        }

        public static ShieldApplication CreateInstance(RestClient client)
        {
            return new ShieldApplication(client, null);
        }

        public static ShieldApplication CreateInstance(RestClient client, ShieldClient parent)
        {
            return new ShieldApplication(client, parent);
        }

        /// <summary>
        /// Upload an application to project
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="dependencies">Required dependencies list</param>
        /// <param name="projectKey">Project key (where application will be uploaded)</param>
        /// <returns></returns>
        public async Task<DirectUploadDto> UploadApplicationDirectlyAsync(string projectKey, ShieldFile file, List<ShieldFile> dependencies)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");
                LogHelper.LogInformation("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct".ToApiRoute())
                    .AddJsonBody(projectKey)
                    .AddFile("file", file.FileContent, file.FileName, MimeTypeMap.GetMimeType(file.FileName))
                    .AddDependencies(dependencies?.Select(x => (x.FileContent, x.FileName)).ToList());

                var result = await _client.PostAsync<DirectUploadDto>(request);

                LogHelper.LogInformation($"The application {file.FileName} has been uploaded successfully.");

                return result;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while uploading the {file.FileName} application.");
                // throw new Exception($"An error occurred while uploading the {file.FileName} application: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while uploading the {file.FileName} application.");
                throw new Exception();
            }
        }

        /// <summary>
        /// Upload an application to project
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="dependencies">Required dependencies list</param>
        /// <param name="projectKey">Project key (where application will be uploaded)</param>
        /// <returns></returns>
        public DirectUploadDto UploadApplicationDirectly(string projectKey, ShieldFile file,
            List<ShieldFile> dependencies)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");
                LogHelper.LogInformation("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct".ToApiRoute())
                    .AddJsonBody(projectKey)
                    .AddFile("file", file.FileContent, file.FileName, MimeTypeMap.GetMimeType(file.FileName))
                    .AddDependencies(dependencies?.Select(x => (x.FileContent, x.FileName)).ToList());

                var result = _client.Post<DirectUploadDto>(request);

                // Parent.CustomLogger?.LogDebug($"The application {file.FileName} has been uploaded successfully.");
                LogHelper.LogInformation($"The application {file.FileName} has been uploaded successfully.");

                return result.IsSuccessful ? result.Data : null;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while uploading the {file.FileName} application.");
                // throw new Exception($"An error occurred while uploading the {file.FileName} application: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while uploading the {file.FileName} application.");
                throw new Exception();
            }
        }

        /// <summary>
        /// Upload an application to project
        /// </summary>
        /// <param name="filePath">Path of file to upload</param>
        /// <param name="dependenciesPaths">Required dependencies path list</param>
        /// <param name="projectKey">Project key (where application will be uploaded)</param>
        /// <returns></returns>
        public async Task<DirectUploadDto> UploadApplicationDirectlyAsync(string projectKey, string filePath, List<string> dependenciesPaths)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");
                LogHelper.LogInformation("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct".ToApiRoute())
                    .AddQueryParameter("projectKey", projectKey)
                    .AddFileFromPath(filePath)
                    .AddDependencies(dependenciesPaths);

                var result = await _client.PostAsync<DirectUploadDto>(request);

                // Parent.CustomLogger?.LogDebug( $"The application {Path.GetFileName(filePath)} has been uploaded successfully.");
                LogHelper.LogInformation( $"The application {Path.GetFileName(filePath)} has been uploaded successfully.");

                return result;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while uploading the application.");
                // throw new Exception($"An error occurred while uploading the application: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while uploading the application.");
                throw new Exception();
            }
        }

        /// <summary>
        /// Upload an application to project
        /// </summary>
        /// <param name="filePath">Path of file to upload</param>
        /// <param name="dependenciesPaths">Required dependencies path list</param>
        /// <param name="projectKey">Project key (where application will be uploaded)</param>
        /// <returns></returns>
        public DirectUploadDto UploadApplicationDirectly(string projectKey, string filePath, List<string> dependenciesPaths)
        {
            try
            {
                // Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");
                LogHelper.LogInformation("Initiating the request to upload an application to project.");

                if (null == _client)
                {
                    LogHelper.LogError("Client instance is null");
                    // throw new Exception("Client instance is null");
                }

                var request = new RestRequest("/app/direct".ToApiRoute())
                    .AddQueryParameter("projectKey", projectKey)
                    .AddFileFromPath(filePath)
                    .AddDependencies(dependenciesPaths);

                var result = _client.Post<DirectUploadDto>(request);

                if (!result.IsSuccessful)
                {
                    // throw new Exception($"Deps ({dependenciesPaths.Count}): {JsonConvert.SerializeObject(dependenciesPaths, Formatting.Indented)} - Result: {JsonConvert.SerializeObject(result.Content, Formatting.Indented)}");
                    LogHelper.LogError("Client instance is null");
                    return null;
                }

                // Parent.CustomLogger?.LogDebug( $"The application {Path.GetFileName(filePath)} has been uploaded successfully.");
                LogHelper.LogInformation( $"The application {Path.GetFileName(filePath)} has been uploaded successfully.");

                return result.Data;
                // return result.IsSuccessful ? result.Data : null;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical($"An error occurred while uploading the application: {ex.Message}.");
                // throw new Exception($"An error occurred while uploading the application: {ex.Message}");

                LogHelper.LogException(ex, $"An error occurred while uploading the application.");
                throw new Exception();
            }
        }

        public async Task<byte[]> DownloadApplicationAsArrayAsync(ProtectedApplicationDto protectedApplication)
            => (await DownloadApplicationAsync(protectedApplication.DownloadKey)).ToArray();

        public async Task<byte[]> DownloadApplicationAsArrayAsync(ProtectedApplicationDto protectedApplication,
            DownloadFormat format)
            => (await DownloadApplicationAsync(protectedApplication.DownloadKey, format)).ToArray();

        public async Task<Stream> DownloadApplicationAsStreamAsync(ProtectedApplicationDto protectedApplication,
            DownloadFormat format)
            => await DownloadApplicationAsync(protectedApplication.DownloadKey, format);

        public async Task<Stream> DownloadApplicationAsStreamAsync(ProtectedApplicationDto protectedApplication)
            => await DownloadApplicationAsync(protectedApplication.DownloadKey);

        public async Task<byte[]> DownloadApplicationAsArrayAsync(string downloadKey, DownloadFormat format)
            => (await DownloadApplicationAsync(downloadKey, format)).ToArray();

        public async Task<byte[]> DownloadApplicationAsArrayAsync(string downloadKey)
            => (await DownloadApplicationAsync(downloadKey)).ToArray();

        public async Task<Stream> DownloadApplicationAsStreamAsync(string downloadKey)
            => await DownloadApplicationAsync(downloadKey);

        public async Task<Stream> DownloadApplicationAsStreamAsync(string downloadKey, DownloadFormat format)
            => await DownloadApplicationAsync(downloadKey, format);

        //Non-async

        public byte[] DownloadApplicationAsArray(ProtectedApplicationDto protectedApplication)
            => DownloadApplication(protectedApplication.DownloadKey).ToArray();

        public byte[] DownloadApplicationAsArray(ProtectedApplicationDto protectedApplication, DownloadFormat format)
            => DownloadApplication(protectedApplication.DownloadKey, format).ToArray();

        public Stream DownloadApplicationAsStream(ProtectedApplicationDto protectedApplication, DownloadFormat format)
            => DownloadApplication(protectedApplication.DownloadKey, format);

        public Stream DownloadApplicationAsStream(ProtectedApplicationDto protectedApplication)
            => DownloadApplication(protectedApplication.DownloadKey);

        public byte[] DownloadApplicationAsArray(string downloadKey, DownloadFormat format)
            => DownloadApplication(downloadKey, format).ToArray();

        public byte[] DownloadApplicationAsArray(string downloadKey)
            => DownloadApplication(downloadKey).ToArray();

        public Stream DownloadApplicationAsStream(string downloadKey)
            => DownloadApplication(downloadKey);

        public Stream DownloadApplicationAsStream(string downloadKey, DownloadFormat format)
            => DownloadApplication(downloadKey, format);

        /// <summary>
        /// Download protected application from project
        /// </summary>
        /// <param name="downloadKey"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        internal Task<MemoryStream> DownloadApplicationAsync(string downloadKey,
            DownloadFormat format = DownloadFormat.Default)
        {
            try
            {
                if (string.IsNullOrEmpty(downloadKey))
                {
                    // Parent.CustomLogger?.LogCritical("The download key is necessary when obtaining a file.");
                    LogHelper.LogError("The download key is necessary when obtaining a file.");
                    throw new ArgumentNullException(nameof(downloadKey));
                }


                LogHelper.LogInformation("Initiating the request to download an application.");
                var stream = new MemoryStream();

                async void ResponseWriter(Stream responseStream)
                {
                    using (responseStream)
                    {
                        await responseStream.CopyToAsync(stream);
                    }
                }

                var request = new RestRequest("/app/download".ToApiRoute())
                    {
                        ResponseWriter = ResponseWriter
                    }
                    .AddQueryParameter("key", downloadKey)
                    .AddQueryParameter("format", format == DownloadFormat.Zip ? "zip" : "default");

                var _ = _client.DownloadData(request);

                // Parent.CustomLogger?.LogDebug("The application has been downloaded successfully.");
                LogHelper.LogInformation("The application has been downloaded successfully.");

                return Task.FromResult(stream);
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical("An error occurred while downloading the file.");
                // throw new ArgumentNullException($"An error occurred while downloading the file: {e.Message}");

                LogHelper.LogException(ex, "An error occurred while downloading the file.");
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Download protected application from project
        /// </summary>
        /// <param name="downloadKey"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        internal MemoryStream DownloadApplication(string downloadKey, DownloadFormat format = DownloadFormat.Default)
        {
            try
            {
                if (string.IsNullOrEmpty(downloadKey))
                {
                    // Parent.CustomLogger?.LogCritical("The download key is necessary when obtaining a file.");
                    LogHelper.LogError("The download key is necessary when obtaining a file.");
                    throw new ArgumentNullException(nameof(downloadKey));
                }


                LogHelper.LogInformation("Initiating the request to download an application.");
                var stream = new MemoryStream();
                var request = new RestRequest("/app/download".ToApiRoute())
                    {
                        ResponseWriter = responseStream =>
                        {
                            using (responseStream)
                            {
                                responseStream.CopyTo(stream);
                            }
                        }
                    }
                    .AddQueryParameter("key", downloadKey)
                    .AddQueryParameter("format", format == DownloadFormat.Zip ? "zip" : "default");

                var _ = _client.DownloadData(request);

                // Parent.CustomLogger?.LogDebug("The application has been downloaded successfully.");
                LogHelper.LogInformation("The application has been downloaded successfully.");

                return stream;
            }
            catch (Exception ex)
            {
                // Parent.CustomLogger?.LogCritical("An error occurred while downloading the file.");
                // throw new ArgumentNullException($"An error occurred while downloading the file: {e.Message}");

                LogHelper.LogException(ex, "An error occurred while downloading the file.");
                throw new ArgumentNullException();
            }
        }
    }
}