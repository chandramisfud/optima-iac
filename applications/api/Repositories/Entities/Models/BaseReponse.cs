namespace Repositories.Entities.Models
{
    public class BaseResponse
    {
        public int code { get; set; }
        public bool error { get; set; }
        public string? message { get; set; }
        public object? values { get; set; }
    }
    public class BaseResponseUpload
    {
        public int code { get; set; }
        public bool error { get; set; }
        public string? message { get; set; }
        public object? values { get; set; }
        public object? xlsRowValues { get; set; }
        public object? tableTempValues { get; set; }
    }
    public class BaseResponseFiltered : BaseResponse
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class ImportBudgetResponse
    {
        public string? budget { get; set; }
        public string? status { get; set; }
    }
}
