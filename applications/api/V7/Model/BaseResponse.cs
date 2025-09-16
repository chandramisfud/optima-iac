namespace V7.Model
{
    public class BaseResponse
    {
        public int code { get; set; }
        public bool error { get; set; }
        public string? message { get; set; }
        public object? values { get; set; }
    }

    public class BaseLP
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<Object>? Data { get; set; }
    }
}
