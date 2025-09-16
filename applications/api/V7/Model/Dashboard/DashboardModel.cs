namespace V7.Model.Dashboard
{
   
    public class DashboardMainDNMonitoringParam
    {
        public DateTime period { get; set; }
        public int entityId { get; set; }
        public string? channel { get; set; }
        public string? account { get; set; }
        public string? paymentstatus { get; set; }
        public int distibutorId { get; set; }
        public int dnpromo { get; set; }
    }
}