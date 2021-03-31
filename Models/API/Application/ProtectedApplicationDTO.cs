using System;
using System.Collections.Generic;

namespace Shield.Client.Models.API.Application {
	public class ProtectedApplicationDto {
		public string Name { get; set; }
		public string DownloadKey { get; set; }
		public DateTime Date { get; set; }
		public DateTime Expiration { get; set; }
		public string Preset { get; set; }
		public List<string> ProtectionsUsed { get; set; }
	}
}
