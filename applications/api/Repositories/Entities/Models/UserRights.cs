namespace Repositories.Entities.Models
{
    public class UserRights
    {
        public string? usergroupid { get; set; }
        public string? menuid { get; set; }
        public int create_rec { get; set; }
        public int read_rec { get; set; }
        public int update_rec { get; set; }
        public int delete_rec { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class UserRightsLPData
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? userName { get; set; }
        public string? contactInfo { get; set; }
        public bool isdeleted { get; set; }
    }

    public class UserRightsLP
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<UserRightsLPData>? Data { get; set; }
    }
}
