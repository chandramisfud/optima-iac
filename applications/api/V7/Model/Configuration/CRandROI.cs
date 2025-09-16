namespace V7.Model.Configuration
{
    public class CRandROIParam
    {
        public CRandROIList[]? config { get; set; }
    }

    public class CRandROIList
    {
        public int id { get; set; }
        public double minimumROI { get; set; }
        public double maksimumROI { get; set; }
        public double minimumCostRatio { get; set; }
        public double maksimumCostRatio { get; set; }
    }

    public class ConfigRoiParamDelete
    {
        public int id { get; set; }
    }

    public class ListSubCategory
    {
        public ValueSubCategory[]? data { get; set; }
    }

    public class ValueSubCategory
    {
        public int Id { get; set; }
        public int LongDesc { get; set; }
    }
}
