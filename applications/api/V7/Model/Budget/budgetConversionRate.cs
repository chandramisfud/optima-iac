namespace V7.Model.Budget
{
    public class budgetConversionRateLPParam : LPParamReq
    {
       
            public string? period { get; set; }
            public int[]? channel { get; set; }
            public int[]? subChannel { get; set; }
            public int[]? groupBrand { get; set; }
       
    }

    public class budgetPSInputParam : LPParamReq
    {

        public string? period { get; set; }
        public int[]? distributor { get; set; }
        public int[]? groupBrand { get; set; }

    }
    public class budgetVolumeLPParam : LPParamReq
    {

        public string? period { get; set; }
        public int[]? channel { get; set; }
        public int[]? subChannel { get; set; }
        public int[]? account { get; set; }
        public int[]? subAccount { get; set; }
        public int[]? groupBrand { get; set; }
        public int[]? region { get; set; }

    }

    public class ttConsoleLPParam : LPParamReq
    {

        public string? period { get; set; }
        public int[]? category { get; set; }
        public int[]? subCategory { get; set; }
        public int[]? subActivityType { get; set; }
        public int[]? activity { get; set; }
        public int[]? subActivity { get; set; }
        public int[]? channel { get; set; }
        public int[]? subChannel { get; set; }
        public int[]? account { get; set; }
        public int[]? subAccount { get; set; }
        public int[]? distributor { get; set; }
        public int[]? groupBrand { get; set; }

    }

    public class ttConsoleCreateParam 
    {

        public required string period { get; set; }
        public required int category { get; set; }
        public required int subCategory { get; set; }
        public required int subActivityType { get; set; }
        public required int activity { get; set; }
        public int subActivity { get; set; }
        public required int channel { get; set; }
        public  int subChannel { get; set; }
        public  int account { get; set; }
        public  int subAccount { get; set; }
        public required int distributor { get; set; }
        public required string distributorShortDesc { get; set; }
        public required int groupBrand { get; set; }
        public required string budgetName { get; set; }
        public required decimal tt { get; set; }
       
    }

    public class ttConsoleUpdateParam
    {
        public required int id { get; set; }
        public required string period { get; set; }
        public required int category { get; set; }
        public required int subCategory { get; set; }
        public required int subActivityType { get; set; }
        public required int activity { get; set; }
        public int subActivity { get; set; }
        public required int channel { get; set; }
        public  int subChannel { get; set; }
        public  int account { get; set; }
        public  int subAccount { get; set; }
        public required int distributor { get; set; }
        public required string distributorShortDesc { get; set; }
        public required int groupBrand { get; set; }
        public required string budgetName { get; set; }
        public required decimal tt { get; set; }


    }

}
