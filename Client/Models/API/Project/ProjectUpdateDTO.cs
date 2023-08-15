

namespace Bytehide.Shield.Client.Models.API.Project {
	public class ProjectUpdateDto {
        public string Name { get; set; }
        public string Type { get;  set; }
        public int Expiration { get; set; }
        public bool ExpireDependencies { get; set; }
	}
}
