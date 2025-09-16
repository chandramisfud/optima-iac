namespace Entities.Tools
{
    public class MatrixApprovalPromoProcess
    {
        public int Id { get; set; }
        public required string Date { get; set; }
        public required bool IsFinished { get; set; }
        public required string UserProfile { get; set;}
        public List<MatrixApprovalPromoProcessPromo>? PromoDetail { get; set; }
    }
    public class MatrixApprovalPromoProcessFlat
    {
        public int Id { get; set; }
        public required string Date { get; set; }
        public required bool IsFinished { get; set; }
        public required string UserProfile { get; set; }
        public int? PromoId { get; set; }
        public int?MatrixPromoApprovalId { get; set; }
       
    }
    public class MatrixApprovalPromoProcessPromo
    {
        public int PromoId { get; set; }
    }

    public class SavePromoAttachmentParam
    {
        public int PromoId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
        public string? CreateBy { get; set; }
    }
    public class SearchPromobyRefidDto
    {
        public int id { get; set; }
        public string? RefId { get; set; }
        public string? CreateBy { get; set; }
    }
    public class HierarchyResult
    {
        public int BudgetParentID { get; set; }
        public int BudgetParent { get; set; }
        public int Levels { get; set; }
        public int BudgetId { get; set; }
        public string? BudgetParentDesc { get; set; }
        public double TotalAssignmentAmount { get; set; }
        public string? AssignmentDesc { get; set; }
        public double AssignmentAmount { get; set; }
        public string? Approval { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string? CategoryDesc { get; set; }
        public string? SubCategoryDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public string? SubActivityDesc { get; set; }
        public string? AssignTo { get; set; }
    }
    public class AllocationforAdjustResult
    {
        public string? Periode { get; set; }
        public string? RefId { get; set; }
        public string? BudgetType { get; set; }
        public int DistributorId { get; set; }
        public int PrincipalId { get; set; }
        public double BudgetAmount { get; set; }
        public string? LongDesc { get; set; }
        public string? StatusApproval { get; set; }
    }
    public class ToolsUploadEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class PromoListAttachment
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? row1 { get; set; }
        public string? row2 { get; set; }
        public string? row3 { get; set; }

    }
    public class DCImportTableTemp
    {
        public string? BudgetYear { get; set; }
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

}