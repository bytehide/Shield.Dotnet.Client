using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Shield.Client.Models.API
{
    public enum ConfigurationType
    {
        Project, Solution, Application
    }

    public class ProtectionConfigurationDTO
    {
        public string Name { get; set; }

        [DefaultValue(ConfigurationType.Application)]
        public ConfigurationType ConfigurationType { get; set; }

        public bool InheritFromProject { get; set; }

        public string Preset { get; set; }

        public Dictionary<string, ProtectionRules> Protections { get; set; }

        public string Serialize() => JsonSerializer.Serialize(this);

        public ProtectionConfigurationDTO ParseRules()
        {
            Protections.ToList().ForEach(protection =>
            {
                var rules = new ProtectionRules();

                if (protection.Value is null || protection.Value.Count is 0)
                {
                    Protections[protection.Key] = null;
                    return;
                }

                protection.Value.ToList().ForEach(rule => rules.Add(rule.Key, rule.Value.ToString()));

                Protections[protection.Key] = rules;
            });

            return this;
        }
    }

    public static class Helpers
    {
        #region Internal
        internal static ProtectionConfigurationDTO As(ProtectionConfigurationDTO config, ConfigurationType configurationType)
        {
            config.ConfigurationType = configurationType;
            return config;
        }

        internal static ProtectionConfigurationDTO To(ProtectionConfigurationDTO config, ShieldConfigurationPresets.Presets preset)
        {
            config.Preset = preset.ToPresetString();
            return config;
        }

        internal static ProtectionConfigurationDTO UpdateProtections(ProtectionConfigurationDTO config, params string[] protectionsId)
        {
            config.Preset = "custom";
            config.Protections = protectionsId.ToList().ToDictionary(format => format, format => new ProtectionRules());
            return config;
        }

        internal static ProtectionConfigurationDTO UpdateProtections(ProtectionConfigurationDTO config, Dictionary<string, ProtectionRules> protections)
        {
            config.Preset = "custom";
            config.Protections = protections;
            return config;
        }
        #endregion

        public static ProtectionConfigurationDTO AsProject(this ProtectionConfigurationDTO config)
        => As(config, ConfigurationType.Project);

        public static ProtectionConfigurationDTO AsSolution(this ProtectionConfigurationDTO config)
        => As(config, ConfigurationType.Solution);

        public static ProtectionConfigurationDTO AsApplication(this ProtectionConfigurationDTO config)
        => As(config, ConfigurationType.Application);

        public static ProtectionConfigurationDTO ToOptimized(this ProtectionConfigurationDTO config)
        => To(config, ShieldConfigurationPresets.Presets.Optimized);

        public static ProtectionConfigurationDTO ToBalance(this ProtectionConfigurationDTO config)
        => To(config, ShieldConfigurationPresets.Presets.Balance);

        public static ProtectionConfigurationDTO ToMaximum(this ProtectionConfigurationDTO config)
        => To(config, ShieldConfigurationPresets.Presets.Maximum);

        public static ProtectionConfigurationDTO ToCustom(this ProtectionConfigurationDTO config, params string[] protectionsId)
        => UpdateProtections(config, protectionsId);

        public static ProtectionConfigurationDTO ToCustom(this ProtectionConfigurationDTO config, Dictionary<string, ProtectionRules> protections)
        => UpdateProtections(config, protections);

        public static ProtectionConfigurationDTO Rename(this ProtectionConfigurationDTO config, string name)
        {
            config.Name = name;
            return config;
        }
    }

    public class ProtectionRules : Dictionary<string, object> { }
}

