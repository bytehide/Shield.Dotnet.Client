using System.Collections.Generic;
using Shield.Client.Fr.Models.API.Protections;

namespace Shield.Client.Fr.Models.API.Prepare
{
	public class PrepareDto
	{
		public List<ProtectionDto> Protections { get; set; }
		public List<JsonModule> JsonModule { get; set; }
		public string Key { get; set; }
	}
}
