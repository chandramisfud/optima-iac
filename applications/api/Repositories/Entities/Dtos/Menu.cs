namespace Repositories.Entities.Dtos
{

    public class MainMenu
    {
        public Menu? menu { get; set; }
        public List<UserAccess>? user_access { get; set; }
    }

    public class UserAccess
    {
        public string? usergroupid { get; set; }
        public string? menuid { get; set; }
        public int read_rec { get; set; }
        public int create_rec { get; set; }
        public int update_rec { get; set; }
        public int delete_rec { get; set; }
    }
    public class Menu
    {
        public List<SubMenu>? menu { get; set; }
    }

    public class SubMenu
    {
        public string? id { get; set; }
        public string? parent { get; set; }
        public string? name { get; set; }
        public string? icon { get; set; }
        public string? url { get; set; }
        public int number { get; set; }
        public string? flag { get; set; }
        public string? slug { get; set; }
        public string? usergroupid { get; set; }
        public string? menuid { get; set; }
        public int read_rec { get; set; }
        public int create_rec { get; set; }
        public int update_rec { get; set; }
        public int delete_rec { get; set; }
        public List<SubMenu>? submenu { get; set; }
    }



}
