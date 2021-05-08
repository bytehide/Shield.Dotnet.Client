namespace Shield.Client.Fr.Models.API.Edition {
	public class FilePermissionsDto {
		public string CurrentEdition { get; set; }
		public int MaxFiles { get; set;  }
		public int MaxDependencies { get; set; }
		public long MaxFileSize { get; set; }
	}
}
