using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Mapping
{
    public class SKUBlitzPostBody
    {
        public int SKUId { get; set; }
        public string? SAPCode { get; set; }
    }

    public class SKUBlitzDeleteBody
    {
        public int id { get; set; }
    }
}
