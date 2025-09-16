using System.Data;
using V7.Model.Promo;

namespace V7.Model
{
    public class BudgetV2ListParam : LPParamReq
    {
        public int period { get; set; }
        public int[]? category { get; set; }
        public int[]? channelId { get; set; } 

        public int[]? groupBrand { get; set; } 
        public int? is5Bio { get; set; }
        public string[] budgetApprovalStatus { get; set; }
        public int[]? month { get; set; }

 
    }

    public class BudgetApprovalV2DownloadParam
    {
        public int period { get; set; }
        public int[]? category { get; set; }
        public int[]? channelId { get; set; }

        public int[]? groupBrand { get; set; }
        public int? is5Bio { get; set; }
        public string[] budgetApprovalStatus { get; set; }
        public int[]? month { get; set; }
    }
    public class BudgetV2DeployListParam
    {
        public int period { get; set; }
        public int category { get; set; }
        public int[]? channelId { get; set; }

        public int[]? groupBrand { get; set; }
        public int[]? subActivityTypeId { get; set; }
      //  public int? is5Bio { get; set; }
     
    }
    public class BudgetV2ReportParam
    {
        public int period { get; set; }
        public int category { get; set; }
        public int[]? channelId { get; set; }

        public int[]? groupBrand { get; set; }
        public string[] budgetApprovalStatus { get; set; }
        public int[]? month { get; set; }
        public int? is5Bio { get; set; }

    }

    //public class BudgetApprovalV2DownloadParam
    //{
    //    public int period { get; set; }
    //    public int[]? channelId { get; set; }

    //    public int[]? groupBrand { get; set; }
    //    public string[] budgetApprovalStatus { get; set; }
    //    public string batchId { get; set; } = string.Empty;

    //}
    public class BudgetV2PromoArrayIntType
    {
        public int[] promoid { get; set; }

    }

    public class BudgetV2RequestApprove
    {
        public string batchId { get; set; }
        public string profileId { get; set; }
        public string profileEmail { get; set; }

    }

    public class BudgetV2DeployBatchParam
    {
        public string batchId { get; set; }
    }
}
