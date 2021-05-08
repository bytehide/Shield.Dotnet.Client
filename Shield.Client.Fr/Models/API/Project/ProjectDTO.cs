using System;
using System.Collections.Generic;
using Shield.Client.Fr.Models.API.Application;

namespace Shield.Client.Fr.Models.API.Project {
	public class ProjectDto {
		public int Id { get;  set; }
		public string Key { get;  set; }
        public string Name { get;  set; }
        public string Type { get;  set; }
		public string Container { get;  set; }
		public DateTime Date { get; set; }
        public int Expiration { get; set; }
        public bool ExpireDependencies { get; set; }
		public ProjectPermissionsDto Permissions { get; set; }
		public List<ApplicationDto> Applications { get; set; }
		public DateTime? LastRun { get; set; }
	}
}
