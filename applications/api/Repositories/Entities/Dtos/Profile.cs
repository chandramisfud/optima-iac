namespace Repositories.Entities.Dtos
{
    public class ChangeProfileResponse
    {     
        public string? Token { get; set; } 
        public string? UserGroupName { get; set; }
        public string? UserGroupID { get; set; }
        public string? UserLevel { get; set; }
        public string? UserProfileId { get; set; }
        public DateTime TokenExpiredTime { get; set; }
        // added andrie Sept 27 2023 E2 #33
        public List<ProfileCategory>? ProfileCategories { get; set; }
    }
}
