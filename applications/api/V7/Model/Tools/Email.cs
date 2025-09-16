namespace V7.Model
{
    #pragma warning disable CS1591
    public class EmailParam
    {
        public string[]? email { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; }
        public string[]? cc { get; set; }
        public string[]? bcc { get; set; }
        public List<IFormFile>? attachment { get; set; }

    }
}
