using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Shield.Client.Models;
using Shield.Client.Models.API;
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

        /// <summary>
        /// Creates a default protection configuration
        /// </summary>
        /// <param name="preset"></param>
        /// <param name="configurationType"></param>
        /// <returns></returns>
        public ProtectionConfigurationDTO Default(ShieldConfigurationPresets.Presets preset = ShieldConfigurationPresets.Presets.Balance, ConfigurationType configurationType = ConfigurationType.Application)
        => new() { Name= "Default", Preset= preset.ToPresetString(), InheritFromProject= false, ConfigurationType= configurationType };

        /// <summary>
        /// Creates a protection configutation from a protections ids list
        /// </summary>
        /// <param name="protectionsId"></param>
        /// <returns></returns>
        public ProtectionConfigurationDTO FromProtections(params string[] protectionsId)
        => new() { Preset = "custom", Protections = protectionsId.ToList().ToDictionary(format => format, format => new ProtectionRules()) };

        /// <summary>
        /// Creates a protection configuration from a protection-rules list
        /// </summary>
        /// <param name="protections"></param>
        /// <returns></returns>
        public ProtectionConfigurationDTO FromProtections(Dictionary<string, ProtectionRules> protections)
        => new() { Preset = "custom", Protections = protections };

        /// <summary>
        /// Loads a protection configuration from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ProtectionConfigurationDTO LoadConfigurationFromFile(string path)
        {
            if (!File.Exists(path))
                return null;
            try {
                return JsonSerializer.Deserialize<ProtectionConfigurationDTO>(File.ReadAllText(path));
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Loads a protection configuration from a file or creates a default
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ProtectionConfigurationDTO LoadConfigurationFromFileOrDefault(string path)
        => LoadConfigurationFromFile(path) ?? Default();


        internal List<string> DiscoverFiles(string directory, string applicationName)
        {
            if (!Directory.Exists(directory))
                return null;

            var filePaths = Directory.GetFiles(directory,
                $"shield.{(string.IsNullOrEmpty(applicationName) ? "*" : applicationName)}.config.json",
                SearchOption.AllDirectories).ToList();

            if (filePaths.Count == 0)
                filePaths = Directory.GetFiles(directory, $"shield.config.json",
                SearchOption.AllDirectories).ToList();

            if (filePaths.Count == 0)
                return null;

            return filePaths;
        } 

        /// <summary>
        /// Looks for a configuration file in a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="applicationName">leave it null or empty for default configurations</param>
        /// <returns></returns>
        public ProtectionConfigurationDTO FindConfiguration(string directory, string applicationName = "*")
        {
            var files = DiscoverFiles(directory, applicationName);
            if (files is null)
                return null;
            return LoadConfigurationFromFile(files.FirstOrDefault());
        }

        /// <summary>
        /// Looks for a configuration file in a directory or creates a default
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="applicationName">leave it null or empty for default configurations</param>
        /// <returns></returns>
        public ProtectionConfigurationDTO FindConfigurationOrDefault(string directory, string applicationName = "*")
        {
            var files = DiscoverFiles(directory, applicationName);
            if (files is null)
                return null;
            return LoadConfigurationFromFileOrDefault(files.FirstOrDefault());
        }

        #region Legacy

        public ProjectConfigurationDto MakeProjectConfiguration(ShieldConfigurationPresets.Presets preset, string overWriteEdition = null)
           => new ProjectConfigurationDto { ProjectPreset = preset.ToPresetString(), OverwriteEdition = overWriteEdition };

        public ProjectConfigurationDto MakeProjectCustomConfiguration(params string[] protectionsId)
            => new ProjectConfigurationDto { ProjectPreset = "custom", Protections = protectionsId.ToList() };

        public ApplicationConfigurationDto MakeApplicationCustomConfiguration(params string[] protectionsId)
            => new ApplicationConfigurationDto { ProjectPreset = "custom", Protections = protectionsId.ToList(), InheritFromProject = false };

        public ProjectConfigurationDto MakeProjectCustomConfiguration(string overWriteEdition, params string[] protectionsId)
            => new ProjectConfigurationDto { ProjectPreset = "custom", Protections = protectionsId.ToList(), OverwriteEdition = overWriteEdition };

        public ApplicationConfigurationDto MakeApplicationCustomConfiguration(string overWriteEdition, params string[] protectionsId)
            => new ApplicationConfigurationDto { ProjectPreset = "custom", Protections = protectionsId.ToList(), InheritFromProject = false, OverwriteEdition = overWriteEdition };

        public ApplicationConfigurationDto MakeApplicationConfiguration(ShieldConfigurationPresets.Presets preset, string overWriteEdition = null)
            => new ApplicationConfigurationDto { ProjectPreset = preset.ToPresetString(), InheritFromProject = false, OverwriteEdition = overWriteEdition };

        //public async Task<ApplicationConfigurationDto> LoadApplicationConfigurationFromFileAsync(string path)
        //    => JsonConvert.DeserializeObject<ApplicationConfigurationDto>(await File.ReadAllTextAsync(path));

        public ApplicationConfigurationDto LoadApplicationConfigurationFromFile(string path)
            => JsonSerializer.Deserialize<ApplicationConfigurationDto>(File.ReadAllText(path));

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
                return !File.Exists(path) ? MakeApplicationConfiguration(ShieldConfigurationPresets.Presets.Balance) : JsonSerializer.Deserialize<ApplicationConfigurationDto>(File.ReadAllText(path));
            }
            catch
            {
                return MakeApplicationConfiguration(ShieldConfigurationPresets.Presets.Balance);
            }
        }
        //public async Task<ProjectConfigurationDto> LoadProjectConfigurationFromFileAsync(string path)
        //    => JsonConvert.DeserializeObject<ProjectConfigurationDto>(await File.ReadAllTextAsync(path));
        public ProjectConfigurationDto LoadProjectConfigurationFromFile(string path)
            => JsonSerializer.Deserialize<ProjectConfigurationDto>(File.ReadAllText(path));

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
                return !File.Exists(path) ? MakeProjectConfiguration(ShieldConfigurationPresets.Presets.Balance) : JsonSerializer.Deserialize<ProjectConfigurationDto>(File.ReadAllText(path));
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

        #endregion
    }
}
