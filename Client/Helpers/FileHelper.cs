using System.Collections.Generic;
using System.IO;
using RestSharp;

namespace Bytehide.Shield.Client.Helpers
{
    public static class FileHelper
    {
        public static IRestRequest AddFileFromPath(this IRestRequest request, string path)
        {
            request.AddFile("file", path, MimeTypeMap.GetMimeType(Path.GetExtension(path)));
            return request;
        }

        public static IRestRequest AddMultipleFiles(this IRestRequest request,
            List<(string name, string path, string contentType)> files)
        {
            files.ForEach(file => request.AddFile(file.name, file.path, file.contentType));
            return request;
        }

        public static IRestRequest AddDependencies(this IRestRequest request, List<string> filesPath)
        {
            if (filesPath is null)
                return request;
            var (zipContent, name, contentType) = DependenciesHelper.PackDependenciesToZip(filesPath);
            request.AddFile("dependencies", zipContent, name, contentType);
            return request;
        }

        public static IRestRequest AddDependencies(this IRestRequest request, List<(byte[] content, string name)> files)
        {
            if (files is null)
                return request;
            var (zipContent, name, contentType) = DependenciesHelper.PackDependenciesToZip(files);
            request.AddFile("dependencies", zipContent, name, contentType);
            return request;
        }

        public static IRestRequest AddMultipleFiles(this IRestRequest request,
            List<(string name, byte[] content, string fileName, string contentType)> files)
        {
            files.ForEach(file => request.AddFile(file.name, file.content, file.fileName, file.contentType));
            return request;
        }
    }
}