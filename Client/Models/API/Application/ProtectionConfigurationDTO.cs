using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shield.Client.Models.API.Application
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

        public Dictionary<string, Dictionary<string,object>> Protections { get; set; }

    }
}
    
