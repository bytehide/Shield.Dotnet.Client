using System.Collections.Generic;

namespace Shield.Client.Fr.Models.API.Protections {
	public class ProtectionParamsDto {
		public string PrepareKey { get; set; }
		public List<ProtectionDto> SelectedProtections { get; set; }
	}
}
