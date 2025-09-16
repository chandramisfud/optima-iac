namespace Repositories.Entities.Models
{
    public class InvestmentReportModel
    {
        public string? BudgetParent { get; set; }
        public int Levels { get; set; }
        public string? BudgetId { get; set; }
        public string? BudgetType { get; set; }
        public string? BudgetName { get; set; }
        public double InitialBudgetAmount { get; set; }
        public double TotalSpending { get; set; }
        public double DnClaim { get; set; }
        public double Available { get; set; }
    }
    public class InvestmentType
    {
        public int id { get; set; }
        public int SubactivityId { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
    }
    public class InvestmentBodyReq
    {
        public int subactivityid { get; set; }
    }
    public class InvestmentTypeLP
    {
        public int id { get; set; }
        public string? InvestmentTypeCode { get; set; }
        public string? InvestmentType { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class InvestmentTypeBodyReq
    {
        public int isnew { get; set; }
        public int id { get; set; }
        public string? InvestmentTypeCode { get; set; }
        public string? InvestmentType { get; set; }
        public string? userid { get; set; }
    }

    public class InvestmentResultMap
    {
        public int id { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubactivityId { get; set; }
        public string? Subactivity { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeCode { get; set; }
        public string? InvestmentType { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public int IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int InvestmentTypeId_bfr { get; set; }
        public string? InvestmentTypeCode_bfr { get; set; }
        public string? InvestmentType_bfr { get; set; }
    }

    public class GetInvestmentBodyReq
    {
        public int id { get; set; }
    }

    public class TypeLPBody
    {
        public int isdeleted { get; set; }
    }
    public class SetRoiCrTypeInvestment
    {
        public int Id { get; set; }
        public double MinimumROI { get; set; }
        public double MaksimumROI { get; set; }
        public double MinimumCostRatio { get; set; }
        public double MaksimumCostRatio { get; set; }

    }
    public class RoiStoreInvestment
    {
        public IList<SetRoiCrTypeInvestment>? Config { get; set; }
        public string? UserId { get; set; }
    }
}