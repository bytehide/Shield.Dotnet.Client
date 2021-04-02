using System;
using System.Collections.Generic;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Protections;

namespace Shield.Client.Models.API
{
    public class ExceptionsDTO
    {
        public LoadedApplicationDto OriginalApplication { get; set; }
        public List<ProtectionDto> UsedProtections { get; set; }
        public DateTime StarTime { get; set; }
    }
}
