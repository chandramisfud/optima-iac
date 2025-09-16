namespace Repositories.Entities.Model
{
    public class Login
    {
        /// <summary>
        /// user email for id
        /// </summary>
        public string? Id { get; set; }
        public string? Password { get; set; }
    }


    public class LoginReturn
    {
        public string? id { get; set; }
        public string? userName { get; set; }
        public object? userGroupId { get; set; }
        public int userLevel { get; set; }
        public int isLogin { get; set; }
        public int cnt { get; set; }
        public DateTime password_Change { get; set; }
        public object? groupMenuPermission { get; set; }
        public string? email { get; set; }
        public bool userNew { get; set; }
        public int loginFailedCount { get; set; }
        public DateTime loginFailedlastTime { get; set; }
        public int loginFreezeTime { get; set; }
        /// <summary>
        /// list of profile that belong to user
        /// </summary>
        public List<Repositories.Entities.Dtos.UserProfile>? profiles { get; set; }

        /// <summary>
        /// authentication based on JWT token
        /// </summary>
        public string? token { get; set; }
    }

}
