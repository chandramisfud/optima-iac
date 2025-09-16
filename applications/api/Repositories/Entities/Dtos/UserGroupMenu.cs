namespace Repositories.Entities.Dtos
{
    public class UserGroup
    {
		public string? usergroupid { get; set; }
		public string? usergroupname { get; set; }
		public int groupmenupermission { get; set; }
	}
    public class UserGroupMenu
    {
		public string? usergroupid { get; set; }
		public string? usergroupname { get; set; }
		public string? userinput { get; set; }
		public DateTime dateinput { get; set; }
		public string? useredit { get; set; }
		public DateTime dateedit { get; set; }
		public int groupmenupermission { get; set; }
		// get from tbset_config_dropdown
		public string? groupmenupermissionname { get; set; }
		public string? CreatedEmail { get; set; }
		public string? ModifiedEmail { get; set; }
		public string? DeletedEmail { get; set; }
		public string? DeletedBy { get; set; }
		public DateTime DeletedOn { get; set; }
		public bool IsDeleted { get; set; }
	}
}
