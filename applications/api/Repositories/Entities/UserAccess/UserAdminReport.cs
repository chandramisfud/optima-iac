using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.UserAccess
{
    public class UseradminRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class UserAdminReportLandingPage
    {
        public int totalCount{ get; set; }
        public int filteredCount{ get; set; }
        public IList<UserAdminReportData>? Data { get; set; }
    }

    public class UserAdminReportData
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? contactinfo { get; set; }
        public string? distributorid { get; set; }
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public string? userlevel { get; set; }
        public string? levelname { get; set; }
        public string? menuid { get; set; }
        public string? menu { get; set; }
        public string? submenu { get; set; }
        public int flag { get; set; }
        public int crud { get; set; }
        public int approve { get; set; }
        public int create_rec { get; set; }
        public int read_rec { get; set; }
        public int update_rec { get; set; }
        public int delete_rec { get; set; }
    }

}
