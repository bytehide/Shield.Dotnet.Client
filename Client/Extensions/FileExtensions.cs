using System.IO;
using System.Threading.Tasks;

namespace Bytehide.Shield.Client.Extensions
{
    public static class FileExtensions
    {
        /// <summary>
        /// Save downloaded application as stream to path.
        /// </summary>
        /// <param name="downloadedApplication"></param>
        /// <param name="path"></param>
        /// <param name="replaceIfExist">Replace if the path contains an existing file.</param>
        /// <returns></returns>
        public static async Task SaveOnAsync(this Stream downloadedApplication, string path, bool replaceIfExist = false)
        {
            if(File.Exists(path))
                if (replaceIfExist)
                    File.Delete(path);
                else return;

            using var fileStream = File.Create(path);
            downloadedApplication.Seek(0, SeekOrigin.Begin);
            await downloadedApplication.CopyToAsync(fileStream);
        }

        /// <summary>
        /// Save downloaded application as stream to path.
        /// </summary>
        /// <param name="downloadedApplication"></param>
        /// <param name="path"></param>
        /// <param name="replaceIfExist">Replace if the path contains an existing file.</param>
        /// <returns></returns>
        public static void SaveOn(this Stream downloadedApplication, string path, bool replaceIfExist = false)
        {
            if(File.Exists(path))
                if (replaceIfExist)
                    File.Delete(path);
                else return;

            using var fileStream = File.Create(path);
            downloadedApplication.Seek(0, SeekOrigin.Begin);
            downloadedApplication.CopyTo(fileStream);
        }


        /// <summary>
        /// Save downloaded application as array to path.
        /// </summary>
        /// <param name="downloadedApplication"></param>
        /// <param name="path"></param>
        /// <param name="replaceIfExist">Replace if the path contains an existing file.</param>
        /// <returns></returns>
        public static void SaveOn(this byte[] downloadedApplication, string path, bool replaceIfExist = false)
        {
            if (File.Exists(path))
                if (replaceIfExist)
                    File.Delete(path);
                else return;

            File.WriteAllBytes(path, downloadedApplication);
            
        }
        /// <summary>
        /// Save downloaded application as array to path.
        /// </summary>
        /// <param name="downloadedApplication"></param>
        /// <param name="path"></param>
        /// <param name="replaceIfExist">Replace if the path contains an existing file.</param>
        /// <returns></returns>
        public static async Task SaveOnAsync(this byte[] downloadedApplication, string path, bool replaceIfExist = false)
        {
            await Task.Run(() =>
            {
                if (File.Exists(path))
                    if (replaceIfExist)
                        File.Delete(path);
                    else return;

                // Netstandar 2.2
                //File.WriteAllBytesAsync(path, downloadedApplication);

                File.WriteAllBytes(path, downloadedApplication);
            });
        }
    }
}
