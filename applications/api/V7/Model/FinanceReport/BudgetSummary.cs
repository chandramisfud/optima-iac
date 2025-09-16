namespace V7.Model
{
    public class BudgetSummaryParam
    {
        public int period { get; set; }
        public int category { get; set; }
        public int[] channel { get; set; }
        public int[] grpBrand { get; set; }

    }

    public class BudgetSummaryLPParam : LPParamReq
    {
        public int period { get; set; }
        public int category { get; set; }
        public int[] channel { get; set; }
        public int[] grpBrand { get; set; }

    }
}
