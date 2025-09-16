namespace Repositories.Entities.Models
{
    public class DNUploadReturn
    {
        public List<DNUpload>? data { get; set; }
        public DNUploadRecordTotal? totalRecord { get; set; }
    }
    public class DNUpload
    {
        public string? doc { get; set; }
        public string? status { get; set; }
    }

    public class DNUploadRecordTotal
    {
        public int failedRec { get; set; }
        public int successRec { get; set; }
        public int totRec { get; set; }
    }

}