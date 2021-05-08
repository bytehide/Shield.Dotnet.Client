using System.Collections.Generic;
using Shield.Client.Fr.Models.API.Application;

namespace Shield.Client.Fr.Models.API.Protections
{
    public class ProtectionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllowsNative { get; set; }
        public bool Available { get; set; }
        public bool Compatible { get; set; }
        public string Edition { get; set; }
        public string Preset { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public int Intensity { get; set; }
        public string DetailedDescription { get; set; }
        public string GoesWellWith { get; set; }
        public List<string> IncompatibleWith { get; set; }

        public ApplicationExclusionsDto Exclusions { get; set; }
    }
}
