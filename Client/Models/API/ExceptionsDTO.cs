using System;
using System.Collections.Generic;
using Bytehide.Shield.Client.Models.API.Application;
using Bytehide.Shield.Client.Models.API.Protections;

namespace Bytehide.Shield.Client.Models.API
{
    public class ExceptionsDTO
    {
        public LoadedApplicationDto OriginalApplication { get; set; }
        public List<ProtectionDto> UsedProtections { get; set; }
        public DateTime StarTime { get; set; }
    }
}
