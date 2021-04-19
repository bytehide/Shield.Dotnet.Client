using System.IO;
using Newtonsoft.Json;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;

namespace Shield.Client.Extensions
{
    public static class ConfigurationExtensions
    {
        //public static async Task SaveToFileAsync(this ApplicationConfigurationDto config, string path, string applicationName)
        //    => await File.WriteAllTextAsync(Path.Combine(path, $"shield.application.${applicationName}.json"), JsonConvert.SerializeObject(config));

        public static void SaveToFile(this ApplicationConfigurationDto config, string path, string applicationName)
            =>  File.WriteAllText(Path.Combine(path, $"shield.application.{applicationName}.json"), JsonConvert.SerializeObject(config));

        //public static async Task SaveToFileAsync(this ProjectConfigurationDto config, string path, string projectName)
        //    => await File.WriteAllTextAsync(Path.Combine(path,$"shield.project.${projectName}.json"), JsonConvert.SerializeObject(config));

        public static void SaveToFile(this ProjectConfigurationDto config, string path, string projectName)
            =>  File.WriteAllText(Path.Combine(path, $"shield.project.{projectName}.json"), JsonConvert.SerializeObject(config));

    }
}
