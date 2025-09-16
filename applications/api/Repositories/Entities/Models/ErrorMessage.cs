using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ErrorMessageDto
    {
        public bool error { get; set; }
        public string? message { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int statuscode { get; set; }
        public string? userid_approver { get; set; }
        public string? username_approver { get; set; }
        public string? email_approver { get; set; }
        public string? userid_initiator { get; set; }
        public string? username_initiator { get; set; }
        public string? email_initiator { get; set; }
        public bool IsFullyApproved { get; set; }
        public bool IsFullyApprovedRecon { get; set; }
        public bool major_changes { get; set; }

    }
}
