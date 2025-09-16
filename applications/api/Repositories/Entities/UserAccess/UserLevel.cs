using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models
{
	public class UserLevel
	{
		public int userlevel { get; set; }
		public string? levelname { get; set; }
		public string? usergroupid { get; set; }
		public string? usergroupname { get; set; }
		public string? userinput { get; set; }
		public DateTime dateinput { get; set; }
		public string? useredit { get; set; }
		public DateTime dateedit { get; set; }
		public string? CreatedEmail { get; set; }
		public string? ModifiedEmail { get; set; }
		public string? DeleteEmail { get; set; }

	}
    

    public class UserLevelLP
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<UserLevel>? Data { get; set; }
    }

	public class userLevelCreate
	{

		public int userlevel { get; set; }
		public string? levelname { get; set; }
		public string? usergroupid { get; set; }
		
		public string? byUserName { get; set; }
		public string? byUserEmail { get; set; }

	}
}