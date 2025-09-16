namespace V7.Model.Budget
{

    public class BudgetAllocationCreateParam
    {
        public BudgetCreateHeader? budgetHeader { get; set; }
        public Budgetdetail? budgetDetail { get; set; }
        public BudgetRegion[]? regions { get; set; }
        public BudgetChannel[]? channels { get; set; }
        public BudgetSubchannel[]? subChannels { get; set; }
        public BudgetAccount[]? accounts { get; set; }
        public BudgetSubaccount[]? subAccounts { get; set; }
        public BudgetBrand[]? brands { get; set; }
        public BudgetSku[]? skus { get; set; }
        public Useraccess[]? userAccess { get; set; }
    }

    public class BudgetAllocationUpdateParam
    {
        public BudgetUpdateHeader? budgetHeader { get; set; }
        public Budgetdetail? budgetDetail { get; set; }
        public BudgetRegion[]? regions { get; set; }
        public BudgetChannel[]? channels { get; set; }
        public BudgetSubchannel[]? subChannels { get; set; }
        public BudgetAccount[]? accounts { get; set; }
        public BudgetSubaccount[]? subAccounts { get; set; }
        public BudgetBrand[]? brands { get; set; }
        public BudgetSku[]? skus { get; set; }
        public Useraccess[]? userAccess { get; set; }
    }
    public class BudgetCreateHeader
    {
        public string? periode { get; set; }
        public string? budgetType { get; set; }
        public int distributorId { get; set; }
        public string? ownerId { get; set; }
        public string? fromOwnerId { get; set; }
        public int budgetMasterId { get; set; }
        public int budgetSourceId { get; set; }
        public double salesAmount { get; set; }
        public double budgetAmount { get; set; }
        public string? longDesc { get; set; }
        public string? shortDesc { get; set; }
    }

    public class BudgetUpdateHeader :BudgetCreateHeader
    {
        public int id { get; set; }
    }
        public class Budgetdetail
    {
        public int allocationId { get; set; }
        public int lineIndex { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public double budgetAmount { get; set; }

        public string? longDesc { get; set; }
    }

    public class BudgetRegion
    {
        public int id { get; set; }
    }

    public class BudgetChannel
    {
        public int id { get; set; }
    }

    public class BudgetSubchannel
    {
        public int id { get; set; }
    }

    public class BudgetAccount
    {
        public int id { get; set; }
    }

    public class BudgetSubaccount
    {
        public int id { get; set; }
    }

    public class BudgetBrand
    {
        public int id { get; set; }
    }

    public class BudgetSku
    {
        public int id { get; set; }
    }

    public class Useraccess
    {
        public string? id { get; set; }
    }

}
