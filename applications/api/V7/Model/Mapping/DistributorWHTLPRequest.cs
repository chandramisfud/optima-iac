
using Repositories.Entities.Models;
using V7.Model.UserAccess;

namespace V7.Model
{
    public class DistributorWHTLPRequest : LPParam
    {
        public string distributor { get; set; } = String.Empty;
        public string subActivity { get; set; } = String.Empty;
        public string subAccount { get; set; } = String.Empty;
        public string WHTType { get; set; } = String.Empty;

        /// <summary>
        /// sort data by any column name showing
        /// </summary>        
        public string? sortColumn { get; set; }
        public sortDirection sortDirection { get; set; }
    }

    public class DistributorWHTUpdateParam
    {
        public int id { get; set; }
        public required string WHTType { get; set; } 
    }
    public class DistributorWHTDeleteParam
    {
        public int id { get; set; }
    }

    public class DistributorWHTCreateParam
    {
        public required string distributor { get; set; }
        public required string subActivity { get; set; }
        public required string subAccount { get; set; }
        public required string WHTType { get; set; }
    }
}
