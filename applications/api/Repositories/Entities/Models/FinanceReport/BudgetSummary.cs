using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class BrandSummaryModel
    {
        public string brand { get; set; }
        public BudgetSummaryDataModel grandTotal { get; set; }
        public List<BrandSummaryChannelModel> channels { get; set; }
    }


    public class BrandSummaryChannelModel
    {
        public string channel { get; set; }
        public BudgetSummaryDataModel subTotal { get; set; }
        public List<BrandSummaryAccountModel> accounts { get; set; }
    }

    public class BrandSummaryAccountModel
    {
        public string account { get; set; }
        public BudgetSummaryDataModel total { get; set; }
    }

    public class BudgetSummaryDataModel
    {
        public string brand { get; set; }
        public string channel { get; set; }
        public string subChannel { get; set; }
        public string account { get; set; }
        public string subAccount { get; set; }
        public decimal ssVolumeTons { get; set; }
        public decimal psVolumeTons { get; set; }
        public decimal ss { get; set; }
        public decimal ps { get; set; }
        public decimal kpi { get; set; }
        public decimal kpiPct { get; set; }
        public decimal rga { get; set; }
        public decimal rgaPct { get; set; }
        public decimal transport { get; set; }
        public decimal transportPct { get; set; }
        public decimal otherCost { get; set; }
        public decimal otherCostPct { get; set; }
        public decimal totalDistributorCost { get; set; }
        public decimal totalDistributorCostPct { get; set; }
        public decimal tt { get; set; }
        public decimal pctTtToSs { get; set; }
        public decimal pctTtToPs { get; set; }
        public decimal adhoc { get; set; }
        public decimal pctAdhocToSs { get; set; }
        public decimal pctAdhocToPs { get; set; }
        public decimal ttPlusAdhoc { get; set; }
        public decimal pctTtPlusAdhocTotSs { get; set; }
        public decimal pctTtPlusAdhocTotPs { get; set; }
        public decimal totalTradeSpend { get; set; }
        public decimal tradeSpendPctToPs { get; set; }
        public decimal warChest { get; set; }
        public decimal warChestPctToPs { get; set; }
        public decimal totalTS { get; set; }
        public decimal pctToPs { get; set; }
    }

    public class DCBrandModel
    {
        public string brand { get; set; }
        public BudgetSummaryDCDataModel grandTotal { get; set; }
        public List<subActivityTypeModel> subActivityType { get; set; }
    }
    public class subActivityTypeModel
    {
        public string subActivityType { get; set; }
        public BudgetSummaryDCDataModel subTotal { get; set; }
    }
    public class BudgetSummaryDCDataModel
    {
        public string brand { get; set; }
        public string subActivityType { get; set; }
        public decimal ssVolumeTons { get; set; }
        public decimal ss { get; set; }
        public decimal ps { get; set; }
        public decimal rrTrs { get; set; }
        public decimal rrPtt { get; set; }
        public decimal rrApl { get; set; }
        public decimal rrAld { get; set; }
        public decimal rrTotal { get; set; }
        public decimal nrTrs { get; set; }
        public decimal nrPtt { get; set; }
        public decimal nrApl { get; set; }
        public decimal nrAld { get; set; }
        public decimal nrTotal { get; set; }
        public decimal totTrs { get; set; }
        public decimal totPtt { get; set; }
        public decimal totApl { get; set; }
        public decimal totAld { get; set; }
        public decimal totTotal { get; set; }
    }


    public class DistributorModel
    {
        public string distributor { get; set; }
        public BudgetSummaryDCDataModel total { get; set; }
    }

   

}
