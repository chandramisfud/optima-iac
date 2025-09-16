namespace Repositories.Entities.Models
{
    public class User
    {
        public string? Id { get; set; } = null!;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Usergroupid { get; set; } = null!;
        public int Userlevel { get; set; }
        public string? Department { get; set; }
        public string? Jobtitle { get; set; }
        public string? Contactinfo { get; set; }
        public string? Distributorid { get; set; }
        public int Registered { get; set; }
        public string? Code { get; set; }
        public DateTime? PasswordChange { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenDate { get; set; }
        public string? Userinput { get; set; }
        public DateTime? Dateinput { get; set; }
        public string? Useredit { get; set; }
        public DateTime? Dateedit { get; set; }
        public int? Isdeleted { get; set; }
        public string? Deletedby { get; set; }
        public DateTime? Deletedon { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? IsLogin { get; set; }
        public int? Usernew { get; set; }
        public int? LoginFailedCount { get; set; }
        public DateTime? LoginFailedLastTime { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
        public string? PictureProfileFile { get; set; }
    }

    public class UserLPData
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? userName { get; set; }
        public string? contactInfo { get; set; }
        public string? status { get; set; }
        public string? profileuser { get; set; }
    }

    public class UserByID
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? userName { get; set; }
        public string? contactInfo { get; set; }
        public string? status { get; set; }
        public List<UserByIDProfile>? profiles { get; set; }
    }

    public class UserByIDProfile
    {
        public int id { get; set; }
        public string? profileid { get; set; }
    }
    public class UserLP
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<UserLPData>? Data { get; set; }
    }

    public class UserProfileList
    {
        public string? id { get; set; }
    }
}
