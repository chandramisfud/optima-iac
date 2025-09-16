using System.ComponentModel.DataAnnotations;

namespace V7.Model.Report
{
    public class TTControlRCDCParam
    {
        public string period { get; set; }
        public int[] categoryId { get; set; }
        public int[] groupBrandId { get; set; }
        public int[] channelId { get; set; }
        public int[] subActivityTypeId { get; set; }
        public string? filter { get; set; }
        public string? search { get; set; }
        public int pageSize { get; set; } = 10;
        public int pageNumber { get; set; } = 0;

    }

}
