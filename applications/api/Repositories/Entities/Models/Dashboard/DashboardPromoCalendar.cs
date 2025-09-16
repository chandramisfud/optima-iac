
namespace Repositories.Entities.Models.Dashboard
{
    public class SubCategoryforDashboardPromoCalendar
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? CategoryLongDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }
    public class DashboardCalendarDto
    {
        public int planningid { get; set; }
        public int promoid { get; set; }
        public int sts { get; set; }
        public int warningsts { get; set; }
        public string? activitydesc { get; set; }
        public DateTime startpromo { get; set; }
        public DateTime endpromo { get; set; }
        public double planinvest { get; set; }
        public double promoinvest { get; set; }
        public double totalclaim { get; set; }
        public string? CreateBy { get; set; }
        public DateTime createon { get; set; }
        public string? StatusApproval { get; set; }
        public string? Approver { get; set; }
        public DateTime ApproveOn { get; set; }

    }
}