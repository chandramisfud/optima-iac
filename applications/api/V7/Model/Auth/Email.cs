using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7.Model
{
    //add by andrie, May 31 2023
    public class EmailForgotPassBody
    {
        public string? email { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; }

    }

    //add by andrie, June 6 2023
    public class EmailResetBody
    {
        public int userId { get; set; }
    }
}
