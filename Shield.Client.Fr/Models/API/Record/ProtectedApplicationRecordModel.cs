using System.Collections.Generic;
using Shield.Client.Fr.Models.API.Protections;

namespace Shield.Client.Fr.Models.API.Record
{
    public class ProtectedApplicationRecordModel
    {
        public string Preset { get; set; }
        public List<ProtectionDto> Protections { get; set; }
    }
}
