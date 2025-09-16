namespace Repositories.Entities.Configuration
{
    public class UpdatePromoItem
    {
        public int categoryId { get; set; }
        public string? userid { get; set; }
        public string? useremail { get; set; }
        public PromoItem? ConfigPromoItem { get; set; }

    }

    public class GetPromoItem
    {
        public object? PromoConfig { get; set; }
        public object? EnableConfig { get; set; }
    }

    public class PromoItemConfig
    {
        public int id { get; set; }
        public int categoryId { get; set; }
        public int budgetYear { get; set; }
        public int promoPlanning { get; set; }
        public int budgetSource { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int subCategory { get; set; }
        public int activity { get; set; }
        public int subActivity { get; set; }
        public int subActivityType { get; set; }
        public int startPromo { get; set; }
        public int endPromo { get; set; }
        public int activityName { get; set; }
        public int initiatorNotes { get; set; }
        public int incrSales { get; set; }
        public int investment { get; set; }
        public int channel { get; set; }
        public int subChannel { get; set; }
        public int account { get; set; }
        public int subAccount { get; set; }
        public int region { get; set; }
        public int groupBrand { get; set; }
        public int brand { get; set; }
        public int SKU { get; set; }
        public int mechanism { get; set; }
        public int attachment { get; set; }
        public int ROI { get; set; }
        public int CR { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public string? createdEmail { get; set; }
        public DateTime modifiedOn { get; set; }
        public string? modifiedBy { get; set; }
        public string? modifiedEmail { get; set; }
        public int isDelete { get; set; }
        public DateTime deleteOn { get; set; }
        public string? deleteBy { get; set; }
        public string? deleteEmail { get; set; }
    }

    public class EnablePromoConfig
    {
        public int enabledCategoryId { get; set; }
        public int enabledBudgetYear { get; set; }
        public int enabledPromoPlanning { get; set; }
        public int enabledBudgetSourve { get; set; }
        public int enabledEntity { get; set; }
        public int enabledDistributor { get; set; }
        public int enabledSubCategory { get; set; }
        public int enabledActivity { get; set; }
        public int enabledSubActivity { get; set; }
        public int enabledSubActivityType { get; set; }
        public int enabledStartPromo { get; set; }
        public int enabledEndPromo { get; set; }
        public int enabledActivityName { get; set; }
        public int enabledInitiatorNotes { get; set; }
        public int enabledIncrSales { get; set; }
        public int enabledInvestment { get; set; }
        public int enabledChannel { get; set; }
        public int enabledSubChannel { get; set; }
        public int enabledAccount { get; set; }
        public int enabledSubAccount { get; set; }
        public int enabledRegion { get; set; }
        public int enabledGroupBrand { get; set; }
        public int enabledBrand { get; set; }
        public int enabledSKU { get; set; }
        public int enabledMechanism { get; set; }
        public int enabledAttachment { get; set; }
    }

    public class PromoItem
    {
        public int budgetYear { get; set; }
        public int promoPlanning { get; set; }
        public int budgetSource { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int subCategory { get; set; }
        public int activity { get; set; }
        public int subActivity { get; set; }
        public int subActivityType { get; set; }
        public int startPromo { get; set; }
        public int endPromo { get; set; }
        public int activityName { get; set; }
        public int initiatorNotes { get; set; }
        public int incrSales { get; set; }
        public int investment { get; set; }
        public int channel { get; set; }
        public int subChannel { get; set; }
        public int account { get; set; }
        public int subAccount { get; set; }
        public int region { get; set; }
        public int groupBrand { get; set; }
        public int brand { get; set; }
        public int SKU { get; set; }
        public int mechanism { get; set; }
        public int Attachment { get; set; }
        public int ROI { get; set; }
        public int CR { get; set; }
    }
}