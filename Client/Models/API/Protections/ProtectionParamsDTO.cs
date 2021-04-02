using System.Collections.Generic;
using Shield.Client.Models.API.Protections;

namespace Shield.API.Models.Protections {
	public class ProtectionParamsDto {
		public string PrepareKey { get; set; }
		public List<ProtectionDto> SelectedProtections { get; set; }
	}
}
