using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class ChannelModel
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class ChannelLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<ChannelSelect>? Data { get; set; }

    }
    public class ChannelById
    {
        public int Id { get; set; }
    }


    public class ChannelSelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
    }

    public class ChannelListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public ChannelSortColumn SortColumn { get; set; }
        public ChannelSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChannelSortColumn
    {
        ShortDesc,
        LongDesc,
        RefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChannelSortDirection
    {
        asc,
        desc
    }

    public class ChannelCreate
    {
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class ChannelCreateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateOn { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class ChannelUpdate
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class ChannelUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedBy { get; set; }

        public string? ModifiedOn { get; set; }

        public string? ModifiedEmail { get; set; }
    }

    public class ChannelDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class ChannelDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }
}