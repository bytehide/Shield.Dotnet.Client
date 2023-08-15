using System.Collections.Generic;

namespace Bytehide.Shield.Client.Models.API.Protections {
	public class ProtectionParamsDto {
		public string PrepareKey { get; set; }
		public List<ProtectionDto> SelectedProtections { get; set; }
	}
}
