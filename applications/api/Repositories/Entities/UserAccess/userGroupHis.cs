namespace Repositories.Entities.Models
{
	public class userGroupHis
	{
		public int id { get; set; }
		public string usergroupid { get; set; } = string.Empty;
		public string usergroupname { get; set; } = string.Empty;
		public int groupmenupermission { get; set; }
		public string StatusAction { get; set; } = string.Empty;
		public DateTime ActionOn { get; set; }
		public string ActionBy { get; set; } = string.Empty;
		public string CreatedEmail { get; set; } = string.Empty;

	}

	public class userGroupHisParam
	{
		public string? usergroupid { get; set; }
		public string? statusAction { get; set; }
		public string? userID { get; set; }
		public string? userEmail { get; set; } 
	}
}
