using System.Collections.Generic;
using Bytehide.Shield.Client.Models.API.Protections;

namespace Bytehide.Shield.Client.Models.API.Prepare
{
	public class PrepareDto
	{
		public List<ProtectionDto> Protections { get; set; }
		public List<JsonModule> JsonModule { get; set; }
		public string Key { get; set; }
	}
}
