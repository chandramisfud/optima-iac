using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class InvestmentReportLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<InvestmentReportData>? Data { get; set; }
    }

    public class InvestmentReportData
    {
        public string? Entity { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? Distributor { get; set; }
        public string? BudgetAllocationName { get; set; }
        public string? ActivityType { get; set; }
        public int? IsLastLayer { get; set; }
        public string? Channel { get; set; }
        public double BudgetDeployed { get; set; }
        public double PromoCreated { get; set; }
        public double DNClaimed { get; set; }
        public double DNPaid { get; set; }
        public double ReturnBalanceFromPromo { get; set; }
        public double RemainingBudget { get; set; }
        public double GapBudgetDeployedvsPromoCreated { get; set; }
        public double GapPromoCreatedvsDNClaimed { get; set; }
        public double GapDNClaimedvsDNPaid { get; set; }
        public string? CategoryDesc { get; set; }
    }

    public class InvestmentBudgetAllocationList
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public int Id { get; set; }
        public string? Periode { get; set; }
        public string? RefId { get; set; }
        public string? BudgetType { get; set; }
        public int DistributorId { get; set; }
        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public string? FromOwnerId { get; set; }
        public int BudgetMasterId { get; set; }
        public double BudgetAmount { get; set; }
        public double RemainingBudget { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? StatusApproval { get; set; }
    }

    public class InvestmentEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class InvestmentDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
