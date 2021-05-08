using System;
using System.Collections.Generic;
using Shield.Client.Fr.Models.API.Application;
using Shield.Client.Fr.Models.API.Protections;

namespace Shield.Client.Fr.Models.API
{
    public class ExceptionsDTO
    {
        public LoadedApplicationDto OriginalApplication { get; set; }
        public List<ProtectionDto> UsedProtections { get; set; }
        public DateTime StarTime { get; set; }
    }
}
