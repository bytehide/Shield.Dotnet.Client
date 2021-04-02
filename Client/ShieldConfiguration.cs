using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;

namespace Shield.Client
{
    public class ShieldConfiguration
    {
        public static ShieldConfiguration CreateInstance()
        {
            return new ShieldConfiguration();
        }
        public ProjectConfigurationDto MakeProjectConfiguration(ShieldConfigurationPresets.Presets preset)
            => new ProjectConfigurationDto {ProjectPreset = preset.ToPresetString()};

        public ProjectConfigurationDto MakeProjectCustomConfiguration(params string[] protectionsId)
            => new ProjectConfigurationDto { ProjectPreset = "custom", Protections = protectionsId.ToList() };

        public ApplicationConfigurationDto MakeApplicationCustomConfiguration(params string[] protectionsId)
            => new ApplicationConfigurationDto { ProjectPreset = "custom", Protections = protectionsId.ToList(), InheritFromProject = false};

        public ApplicationConfigurationDto MakeApplicationConfiguration(ShieldConfigurationPresets.Presets preset)
            => new ApplicationConfigurationDto { ProjectPreset = preset.ToPresetString(), InheritFromProject = false };

        //public async Task<ApplicationConfigurationDto> LoadApplicationConfigurationFromFileAsync(string path)
        //    => JsonConvert.DeserializeObject<ApplicationConfigurationDto>(await File.ReadAllTextAsync(path));

        public ApplicationConfigurationDto LoadApplicationConfigurationFromFile(string path)
            => JsonConvert.DeserializeObject<ApplicationConfigurationDto>(File.ReadAllText(path));

        //public async Task<ApplicationConfigurationDto> LoadApplicationConfigurationFromFileOrDefaultAsync(string path)
        //{
        //    try
        //    {
        //        return !File.Exists(path) ? MakeApplicationConfiguration(ShieldConfigurationPresets.Presets.Balance) : JsonConvert.DeserializeObject<ApplicationConfigurationDto>(await File.ReadAllTextAsync(path));
        //    }
        //    catch
        //    {
        //        return MakeApplicationConfiguration(ShieldConfigurationPresets.Presets.Balance);
        //    }
        //}

        public ApplicationConfigurationDto LoadApplicationConfigurationFromFileOrDefault(string path)
        {
            try
            {
                return !File.Exists(path) ? MakeApplicationConfiguration(ShieldConfigurationPresets.Presets.Balance) : JsonConvert.DeserializeObject<ApplicationConfigurationDto>(File.ReadAllText(path));
            }
            catch
            {
                return MakeApplicationConfiguration(ShieldConfigurationPresets.Presets.Balance);
            }
        }
        //public async Task<ProjectConfigurationDto> LoadProjectConfigurationFromFileAsync(string path)
        //    => JsonConvert.DeserializeObject<ProjectConfigurationDto>(await File.ReadAllTextAsync(path));
        public ProjectConfigurationDto LoadProjectConfigurationFromFile(string path)
            => JsonConvert.DeserializeObject<ProjectConfigurationDto>(File.ReadAllText(path));

        //public async Task<ProjectConfigurationDto> LoadProjectConfigurationFromFileOrDefaultAsync(string path)
        //{
        //    try
        //    {
        //        return !File.Exists(path) ? MakeProjectConfiguration(ShieldConfigurationPresets.Presets.Balance) : JsonConvert.DeserializeObject<ProjectConfigurationDto>(await File.ReadAllTextAsync(path));
        //    }
        //    catch
        //    {
        //        return MakeProjectConfiguration(ShieldConfigurationPresets.Presets.Balance);
        //    }
        //}

        public ProjectConfigurationDto LoadProjectConfigurationFromFileOrDefault(string path)
        {
            try
            {
                return !File.Exists(path) ? MakeProjectConfiguration(ShieldConfigurationPresets.Presets.Balance) : JsonConvert.DeserializeObject<ProjectConfigurationDto>(File.ReadAllText(path));
            }
            catch
            {
                return MakeProjectConfiguration(ShieldConfigurationPresets.Presets.Balance);
            }
        }

        //public async Task<ProjectConfigurationDto> FindProjectConfigurationAsync(string directory, string projectName = "*")
        //{
        //    if (!Directory.Exists(directory))
        //        return null;

        //    var filePaths = Directory.GetFiles(directory,
        //        $"shield.project.{(string.IsNullOrEmpty(projectName) ? "*" : projectName)}.json",
        //        SearchOption.AllDirectories).ToList();

        //    if (filePaths.Count == 0)
        //        return null;

        //    return await LoadProjectConfigurationFromFileAsync(filePaths.FirstOrDefault());
        //}

        public ProjectConfigurationDto FindProjectConfiguration(string directory, string projectName = "*")
        {
            if (!Directory.Exists(directory))
                return null;

            var filePaths = Directory.GetFiles(directory,
                $"shield.project.{(string.IsNullOrEmpty(projectName) ? "*" : projectName)}.json",
                SearchOption.AllDirectories).ToList();

            return filePaths.Count == 0 ? null : LoadProjectConfigurationFromFile(filePaths.FirstOrDefault());
        }

        //public async Task<ApplicationConfigurationDto> FindApplicationConfigurationAsync(string directory, string applicationName = "*")
        //{
        //    if (!Directory.Exists(directory))
        //        return null;

        //    var filePaths = Directory.GetFiles(directory, 
        //        $"shield.application.{(string.IsNullOrEmpty(applicationName) ? "*" : applicationName)}.json",
        //        SearchOption.AllDirectories).ToList();

        //    if (filePaths.Count == 0)
        //        return null;

        //    return await LoadApplicationConfigurationFromFileAsync(filePaths.FirstOrDefault());
        //}

        public ApplicationConfigurationDto FindApplicationConfiguration(string directory, string applicationName = "*")
        {
            if (!Directory.Exists(directory))
                return null;

            var filePaths = Directory.GetFiles(directory,
                $"shield.application.{(string.IsNullOrEmpty(applicationName) ? "*" : applicationName)}.json",
                SearchOption.AllDirectories).ToList();

            if (filePaths.Count == 0)
                return null;

            return LoadApplicationConfigurationFromFile(filePaths.FirstOrDefault());
        }
    }
}
