namespace V7.Model.Mapping
{
    public class TaxLevelCreateParam
    {
        public string? MaterialNumber { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Purpose { get; set; }
        public string? Entity { get; set; }
        public int EntityId { get; set; }
        public decimal PPNPct { get; set; }
        public decimal PPHPct { get; set; }
    }
    public class TaxLevelDeleteParam
    {
        public int Id { get; set; }
    }
}