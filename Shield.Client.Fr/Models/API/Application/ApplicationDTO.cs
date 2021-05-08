using System;

namespace Shield.Client.Fr.Models.API.Application {
	public class ApplicationDto {
		public string Name { get; set; }
		public string Icon { get; set; }
		public string Hash { get; set; }
		public long Size { get; set; }
		public string Extension { get; set; }
		public string Framework { get; set; }
		public string Blob { get; set; }
		public DateTime? Expires { get; set; }
        public bool Archived { get; set; }
		public ApplicationConfigurationDto Configuration { get; set; }
	}
}
