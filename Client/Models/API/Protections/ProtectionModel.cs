using System.Collections.Generic;

namespace Shield.Client.Models.API.Protections {
	public class ProtectionModel {
		public string Id { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public IReadOnlyDictionary<string /*Name*/, object /*Value*/> Options { get; set; }
	}
}
