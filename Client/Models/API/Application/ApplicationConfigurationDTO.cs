using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shield.Client.Models.API.Application
{
    public class ApplicationConfigurationDto
    {
        public bool InheritFromProject { get; set; }
        public string ProjectPreset { get; set; }
        [JsonProperty]
        public List<string> Protections { get; set; }
    }
}
