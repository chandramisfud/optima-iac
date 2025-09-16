namespace V7.Model.Mapping
{
    public class PromoReconSubActivityCreateParam
    {
        public int SubActivityId { get; set; }
        public bool AllowEdit { get; set; }
    }
    public class PromoReconSubActivityDeleteParam
    {
        public int SubActivityId { get; set; }
    }

    public class PromoReconSubActivityImportParam
    {
        public string? userid { get; set; }
        public string? useremail { get; set; }
    }
}