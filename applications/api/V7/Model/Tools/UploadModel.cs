namespace V7.Model.Tools
{
    public class UploadLogParam
    {
        public required string activity { get; set; }
        public required string filename { get; set; }
        public required string status { get; set; }
    }
    public class ToolsUploadParam
    {
        public string? userid { get; set; }
        public string? useremail { get; set; }
    }
    public class DCBudgetImportParam
    {
        public string? ProfileId { get; set; }
        public string? useremail { get; set; }
    }
    public class BrandDto
    {
        public string? SeqNo { get; set; }
        public string? Brand { get; set; }
        public string? Sku { get; set; }
        public string? Principal { get; set; }
        public string? userid { get; set; }
    }
    public class SaveModelPromoAttachment
    {
        public int PromoId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
        public string? CreateBy { get; set; }
    }
    public class UploadModelPromoAttachment
    {
        public string? periode { get; set; }
        public int entity { get; set; }

    }
    public class UploadBudgetDCModel
    {
        public string? Periode { get; set; }
        public string? Category { get; set; }
        public string? Distributor { get; set; }
        public string? Brand { get; set; }
        public string? SubActivity { get; set; }
        public decimal BudgetAmount { get; set; }
        public string? BudgetApproval { get; set; }
        public string? UserAccess1 { get; set; }
        public string? UserAccess2 { get; set; }
        public string? UserAccess3 { get; set; }
        public string? UserAccess4 { get; set; }
        public string? UserAccess5 { get; set; }
        public string? UserAccess6 { get; set; }
        public string? UserAccess7 { get; set; }
        public string? UserAccess8 { get; set; }
        public string? UserAccess9 { get; set; }
        public string? UserAccess10 { get; set; }
        public string? UserAccess11 { get; set; }
        public string? UserAccess12 { get; set; }
        public string? UserAccess13 { get; set; }
        public string? UserAccess14 { get; set; }
        public string? UserAccess15 { get; set; }
        public string? UserAccess16 { get; set; }
        public string? UserAccess17 { get; set; }
        public string? UserAccess18 { get; set; }
        public string? UserAccess19 { get; set; }
        public string? UserAccess20 { get; set; }
        public string? UserAccess21 { get; set; }
        public string? UserAccess22 { get; set; }
        public string? UserAccess23 { get; set; }
        public string? UserAccess24 { get; set; }
        public string? UserAccess25 { get; set; }
        public string? UserAccess26 { get; set; }
        public string? UserAccess27 { get; set; }
        public string? UserAccess28 { get; set; }
        public string? UserAccess29 { get; set; }
        public string? UserAccess30 { get; set; }
        public string? UserAccess31 { get; set; }
        public string? UserAccess32 { get; set; }
        public string? UserAccess33 { get; set; }
        public string? UserAccess34 { get; set; }
        public string? UserAccess35 { get; set; }
        public string? ProfileId { get; set; }
    }

    public class UploadPromoAttachmentParam
    {
        public int promoId { get; set; }
    }
}