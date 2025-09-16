namespace V7.Model.Tools
{
    public class PromoApprovalReminderSettingUpdateParam
    {
        public int id { get; set; }
        public int dt1 { get; set; }
        public int dt2 { get; set; }
        public bool eod { get; set; }
        public bool autoRun { get; set; }
    
        public List<PromoApprovalReminderConfigEmail>? configEmail { get; set; }
    }
    public class PromoApprovalReminderConfigEmail
    {

        public string? email { get; set; }
        public string? userName { get; set; }
        public string? userGroupName { get; set; }
        public string? statusName { get; set; }

    }
    public class PromoApprovalReminderManualEmailParam
    {
        public List<PromoApprovalReminderConfigEmail>? configEmail { get; set; }
    }
}
