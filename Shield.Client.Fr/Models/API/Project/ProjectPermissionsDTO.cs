namespace Shield.Client.Fr.Models.API.Project {
	public class ProjectPermissionsDto {
		public bool CanUpdate { get; set; }
		public bool CanDelete { get; set; }
		public bool CanInviteUsers { get; set; }
		public bool CanRemoveUsers { get; set; }
		public bool IsOwner { get; set; }
	}
}
