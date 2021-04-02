using System.Collections.Generic;
using Shield.Client.Models.API.Protections;

namespace Shield.Client.Models.API.Prepare
{
	public class PrepareDto
	{
		public List<ProtectionDto> Protections { get; set; }
		public List<JsonModule> JsonModule { get; set; }
		public string Key { get; set; }
	}
}
