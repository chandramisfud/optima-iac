namespace Repositories.Entities.Models
{
    public class SearchParamDNbyRefidDto
    {
        public int id { get; set; }
        public string? refid { get; set; }
        public string? messageout { get; set; }
    }
    public class DNCreateAttachmentParam
    {
        public int DNId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }

    }
    public class DNCreateAttachmentReturn
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
    }
    public class DNListAttachment
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? row1 { get; set; }
        public string? row2 { get; set; }
        public string? row3 { get; set; }
        public string? row4 { get; set; }
        public string? row5 { get; set; }
        public string? row6 { get; set; }
        public string? row7 { get; set; }
        public string? row8 { get; set; }
        public string? row9 { get; set; }
    }
}