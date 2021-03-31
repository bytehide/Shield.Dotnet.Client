using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shield.Client.Models.API.Project
{
    public class ProjectConfigurationDto
    {
        public string ProjectPreset { get; set; }
        [JsonProperty]
        public List<string> Protections { get; set; }
    }
}
