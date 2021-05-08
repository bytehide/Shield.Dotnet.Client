using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using Shield.Client.Fr.Helpers;
using Shield.Client.Fr.Models;
using Shield.Client.Fr.Models.API.Application;

namespace Shield.Client.Fr
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
        public async Task<DirectUploadDto> UploadApplicationDirectlyAsync(string projectKey, ShieldFile file,
            List<ShieldFile> dependencies)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct")
                    .AddJsonBody(projectKey)
                    .AddFile("file", file.FileContent, file.FileName, MimeTypeMap.GetMimeType(file.FileName))
                    .AddDependencies(dependencies?.Select(x => (x.FileContent, x.FileName)).ToList());

                var result = await _client.PostAsync<DirectUploadDto>(request);

                Parent.CustomLogger?.LogDebug($"The application {file.FileName} has been uploaded successfully.");

                return result;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while uploading the {file.FileName} application.");
                throw new Exception($"An error occurred while uploading the {file.FileName} application: {ex.Message}");
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
                Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct")
                    .AddJsonBody(projectKey)
                    .AddFile("file", file.FileContent, file.FileName, MimeTypeMap.GetMimeType(file.FileName))
                    .AddDependencies(dependencies?.Select(x => (x.FileContent, x.FileName)).ToList());

                var result =  _client.Post<DirectUploadDto>(request);

                Parent.CustomLogger?.LogDebug($"The application {file.FileName} has been uploaded successfully.");

                return result.IsSuccessful ? result.Data : null;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while uploading the {file.FileName} application.");
                throw new Exception($"An error occurred while uploading the {file.FileName} application: {ex.Message}");
            }
        }

        /// <summary>
        /// Upload an application to project
        /// </summary>
        /// <param name="filePath">Path of file to upload</param>
        /// <param name="dependenciesPaths">Required dependencies path list</param>
        /// <param name="projectKey">Project key (where application will be uploaded)</param>
        /// <returns></returns>
        public async Task<DirectUploadDto> UploadApplicationDirectlyAsync(string projectKey, string filePath,
            List<string> dependenciesPaths)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct")
                    .AddQueryParameter("projectKey", projectKey)
                    .AddFileFromPath(filePath)
                    .AddDependencies(dependenciesPaths);

                var result = await _client.PostAsync<DirectUploadDto>(request);

                Parent.CustomLogger?.LogDebug(
                    $"The application {Path.GetFileName(filePath)} has been uploaded successfully.");

                return result;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while uploading the application.");
                throw new Exception($"An error occurred while uploading the application: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Upload an application to project
        /// </summary>
        /// <param name="filePath">Path of file to upload</param>
        /// <param name="dependenciesPaths">Required dependencies path list</param>
        /// <param name="projectKey">Project key (where application will be uploaded)</param>
        /// <returns></returns>
        public DirectUploadDto UploadApplicationDirectly(string projectKey, string filePath,
            List<string> dependenciesPaths)
        {
            try
            {
                Parent.CustomLogger?.LogDebug("Initiating the request to upload an application to project.");

                var request = new RestRequest("/app/direct")
                    .AddQueryParameter("projectKey", projectKey)
                    .AddFileFromPath(filePath)
                    .AddDependencies(dependenciesPaths);

                var result = _client.Post<DirectUploadDto>(request);

                Parent.CustomLogger?.LogDebug(
                    $"The application {Path.GetFileName(filePath)} has been uploaded successfully.");

                return result.IsSuccessful ? result.Data : null;
            }
            catch (Exception ex)
            {
                Parent.CustomLogger?.LogCritical($"An error occurred while uploading the application.");
                throw new Exception($"An error occurred while uploading the application: {ex.Message}");
            }
        }
        public async Task<byte[]> DownloadApplicationAsArrayAsync(ProtectedApplicationDto protectedApplication)
            => (await DownloadApplicationAsync(protectedApplication.DownloadKey)).ToArray();
        public async Task<byte[]> DownloadApplicationAsArrayAsync(ProtectedApplicationDto protectedApplication, DownloadFormat format)
            => (await DownloadApplicationAsync(protectedApplication.DownloadKey, format)).ToArray();

        public async Task<Stream> DownloadApplicationAsStreamAsync(ProtectedApplicationDto protectedApplication, DownloadFormat format)
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
        internal async Task<MemoryStream> DownloadApplicationAsync(string downloadKey, DownloadFormat format = DownloadFormat.Default)
        {
            try
            {
                if (string.IsNullOrEmpty(downloadKey))
                {
                    Parent.CustomLogger?.LogCritical("The download key is necessary when obtaining a file.");
                    throw new ArgumentNullException(nameof(downloadKey));
                }


                Parent.CustomLogger?.LogDebug("Initiating the request to download an application.");
                using var stream = new MemoryStream();
                var request = new RestRequest("/app/download")
                    {
                        ResponseWriter = async responseStream =>
                        {
                            using (responseStream)
                            {
                                await responseStream.CopyToAsync(stream);
                            }
                        }
                    }
                    .AddQueryParameter("key", downloadKey)
                    .AddQueryParameter("format", format == DownloadFormat.Zip ? "zip" : "default");

                var _ = _client.DownloadData(request);

                Parent.CustomLogger?.LogDebug("The application has been downloaded successfully.");

                return stream;
            }
            catch (Exception e)
            {
                Parent.CustomLogger?.LogCritical("An error occurred while downloading the file.");
                throw new ArgumentNullException($"An error occurred while downloading the file: {e.Message}");
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
                    Parent.CustomLogger?.LogCritical("The download key is necessary when obtaining a file.");
                    throw new ArgumentNullException(nameof(downloadKey));
                }


                Parent.CustomLogger?.LogDebug("Initiating the request to download an application.");
                using var stream = new MemoryStream();
                var request = new RestRequest("/app/download")
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

                Parent.CustomLogger?.LogDebug("The application has been downloaded successfully.");

                return stream;
            }
            catch (Exception e)
            {
                Parent.CustomLogger?.LogCritical("An error occurred while downloading the file.");
                throw new ArgumentNullException($"An error occurred while downloading the file: {e.Message}");
            }
        }
    }
}
