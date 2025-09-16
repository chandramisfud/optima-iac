namespace V7.Model.Master
{
    public class InvestmentTypeMappingCreateParam
    {
        public InvestmentTypeMappingCreateList[]? investmentMap { get; set; }
        public string? userid { get; set; }
    }

    public class InvestmentTypeMappingCreateList
    {
        public int id { get; set; }
        public int SubActivityId { get; set; }
        public int InvestmentTypeId { get; set; }
    }
    public class InvestmentTypeMappingDeleteParam
    {
        public InvestmentTypeMappingDeleteList[]? investmentMap { get; set; }
        public string? userid { get; set; }
    }
    public class InvestmentTypeMappingDeleteList
    {
        public int id { get; set; }
        public int SubActivityId { get; set; }
        public int InvestmentTypeId { get; set; }
    }

}