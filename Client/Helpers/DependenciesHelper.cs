using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Bytehide.Shield.Client.Helpers
{
    public static class DependenciesHelper
    {
        /// <summary>
        /// Convert to zip a list of path files
        /// </summary>
        /// <returns></returns>
        public static (byte[] zipContent,string name, string contentType) PackDependenciesToZip(List<string> filesPath)
        {
            byte[] zipFile;

            using (var memoryStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    filesPath.ForEach(file =>
                    {
                        zip.CreateEntryFromFile(file, Path.GetFileName(file));
                    });
                }
                zipFile = memoryStream.ToArray();
            }

            return (zipFile, "dependencies.zip", MimeTypeMap.GetMimeType(".zip"));
        }
        /// <summary>
        /// Convert to zip a list of files
        /// </summary>
        /// <returns></returns>
        public static (byte[] zipContent, string name, string contentType) PackDependenciesToZip(List<(byte[] content,string name)> files)
        {
            byte[] zipFile;

            using (var memoryStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(file =>
                    {
                        var (content, name) = file;
                        var zipItem = zip.CreateEntry(name);

                        using (var fileStream = new MemoryStream(content))
                        {
                            using (var entry = zipItem.Open())
                            {
                                fileStream.CopyTo(entry);
                            }
                        }

                    });
                }
                zipFile = memoryStream.ToArray();
            }
            return (zipFile, "dependencies.zip", MimeTypeMap.GetMimeType(".zip"));
        }
    }

}
