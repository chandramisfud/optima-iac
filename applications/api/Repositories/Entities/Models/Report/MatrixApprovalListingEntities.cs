using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class MatrixApprovalLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<MatrixApprovalData>? Data { get; set; }
    }

    public class MatrixApprovalData
    {
        public string? Periode { get; set; }
        public string? Initiator { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int DistributorId { get; set; }
        public string? Distributor { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? CategoryLongDesc { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? SubActivityType { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannel { get; set; }
        public double MinInvestment { get; set; }
        public double MaxInvestment { get; set; }
        public string? MatrixApprover { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class MatrixApprovalEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class MatrixApprovalDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
