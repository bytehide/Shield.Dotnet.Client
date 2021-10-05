using System.Collections.Generic;

namespace Shield.Client.Fr.Models.API.Project
{
    public class ProjectConfigurationDto
    {
        public string OverwriteEdition { get; set; }
        public string ProjectPreset { get; set; }
        public List<string> Protections { get; set; }
    }
}
