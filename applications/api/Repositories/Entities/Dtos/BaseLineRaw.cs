namespace Repositories.Entities.Dtos
{
    public class BaselineRaw
    {
        public IList<PromoPlanBaseline>? PromoPlanBaseline { get; set; }
        public IList<BaselineCalculation>? BaselineCalculation { get; set; }
        public IList<BaselineRawBSCalculation>? BaselineRawBSCalculation { get; set; }
        public IList<BaselineRawResult>? BaselineRawResult { get; set; }
        public IList<RawActualSales>? RawActualSales { get; set; }
        public IList<RawActualSales>? RawBaseLine { get; set; }
    }

    public class PromoPlanBaseline
    {
        public string? f1 { get; set; }
        public string? f2 { get; set; }
        public string? f3 { get; set; }
        public string? f4 { get; set; }
        public string? f5 { get; set; }
        public string? f6 { get; set; }
        public string? f7 { get; set; }
        public string? f8 { get; set; }
        public string? f9 { get; set; }
    }
    public class BaselineCalculation
    {
        public string? ym { get; set; }
        public double sales { get; set; }
        public double anomaly_pct { get; set; }
        public double bsold { get; set; }
        public double abnormal_pct { get; set; }
        public double sales_abnormal { get; set; }
        public double sales_new { get; set; }
    }
    public class BaselineRawBSCalculation
    {
        public string? bs { get; set; }
        public double bsold { get; set; }
        public double bsnew { get; set; }
    }
    public class BaselineRawResult
    {
        public double baseline_sales { get; set; }
        public double actual_sales { get; set; }
    }
    public class RawActualSales
    {
        public string? Distributor_Id { get; set; }
        public string? Distributor_Name { get; set; }
        public string? Region_Code { get; set; }
        public string? Region_Desc { get; set; }
        public string? Year { get; set; }
        public string? Month_Name { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountCode { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? Product_Code { get; set; }
        public string? Product_Name { get; set; }
        public double Qty_In_Ton { get; set; }
        public double Qty_In_Car { get; set; }
        public double SS_DBP { get; set; }
        public double SS_RBP { get; set; }
        public string? SOURCE { get; set; }
        public int month_int { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string? ym { get; set; }
    }


}