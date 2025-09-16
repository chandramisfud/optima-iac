using Repositories.Entities.Models;

namespace V7.Model.Master
{
    public class MatrixPromoApprovalHistoryListReq : LPParamReq
    {
  
        public int category { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public string? userid { get; set; }
    }
}
