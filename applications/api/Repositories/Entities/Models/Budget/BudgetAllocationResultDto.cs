namespace Entities
{
    public class BudgetAllocationResultDto
    {
       
    }

    public class RegionRes
    {
        public bool flag { get; set; }
        public int RegionAllocId {get;set;}
        public int RegionId {get;set;}
        public string? RegionName {get;set;}
    }
    public class ChannelRes
    {
        public bool flag { get; set; }
        public int ChannelAllocId {get;set;}
        public int ChannelId {get;set;}
        public string? ChannelName {get;set;}
    }
    public class SubChannelRes
    {
        public bool flag { get; set; }
        public int SubChannelAllocId {get;set;}
        public int SubChannelId {get;set; }
        public int ChannelId { get; set; }
        public string? SubChannelName {get;set;}
    }
    public class AccountRes
    {
        public bool flag { get; set; }
        public int AccountAllocId {get;set;}
        public int AccountId {get;set;}
        public int SubChannelId { get; set; }
        public string? AccountName {get;set;}
    }
    public class SubAccountRes
    {
        public bool flag { get; set; }
        public int SubAccountAllocId {get;set;}
        public int SubAccountId {get;set;}
        public int AccountId { get; set; }
        public string? SubAccountName {get;set;}
    }


    public class SubCategoryRes
    {
        public bool flag { get; set; }
        public int SubCategoryAllocId {get;set;}
        public int SubCategoryId {get;set;}
        public string? SubCategoryName {get;set;}
    }

    public class ActivityRes
    {
        public bool flag { get; set; }
        public int ActivityAllocId {get;set;}
        public int ActivityId {get;set;}
        public string? ActivityName {get;set;}
    }

        public class SubActivityRes
    {
        public bool flag { get; set; }
        public int SubActivityAllocId {get;set;}
        public int SubActivityId {get;set;}
        public string? SubActivityName {get;set;}
    }

    public class BrandRes
    {
        public bool flag { get; set; }
        public int BrandAllocId {get;set;}
        public int BrandId {get;set;}
        public string? BrandName {get;set;}
    }

    public class ProductRes
    {
        public bool flag { get; set; }
        public int ProductAllocId {get;set;}
        public int ProductId {get;set;}
        public int BrandId { get; set; }
        public string? ProductName {get;set;}
    }

    public class UserAccessRes
    {
        public bool flag { get; set; }
        public int UserAccAllocId {get;set;}
        public string? UserAccessId {get;set;}
        public string? UserAccessName {get;set;}
    }
    public class UserAssignRes
    {
        public bool flag { get; set; }
        public int UserAssAllocId {get;set;}
        public string? UserAssignId {get;set;}
        public string? UserAssignName {get;set;}
    }
    public class UserPromoRes
    {
        public bool flag { get; set; }
        public int UserProAllocId {get;set;}
        public string? UserPromoId {get;set;}
        public string? UserPromoName {get;set;}
    }
}
