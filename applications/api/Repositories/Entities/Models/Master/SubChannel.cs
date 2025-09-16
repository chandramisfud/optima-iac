using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SubChannelModel
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
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

    public class SubChannelLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SubChannelSelect>? Data { get; set; }
    }
    public class SubChannelById
    {
        public int Id { get; set; }
    }

    public class SubChannelSelect
    {
        public int ChannelId { get; set; }
        public string? ChannelShortDesc { get; set; }
        public string? ChannelLongDesc { get; set; }
        public string? ChannelRefId { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannelShortDesc { get; set; }
        public string? SubChannelLongDesc { get; set; }
        public string? SubChannelRefId { get; set; }
    }

    public class SubChannelListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SubChannelSortColumn SortColumn { get; set; }
        public SubChannelSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubChannelSortColumn
    {
        ChannelId,
        ChannelLongDesc,
        ChannelShortDesc,
        ChannelRefid,
        SubChannelId,
        SubChannelLongDesc,
        SubChannelShortDesc,
        SubChannelRefid
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubChannelSortDirection
    {
        asc,
        desc
    }

    public class SubChannelCreate
    {
        public int ChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class SubChannelCreateReturn
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class SubChannelUpdate
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class SubChannelUpdateReturn
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class SubChannelDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubChannelDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class SubChannelByAccess
    {
        public string? userid { get; set; }

        public int channelid { get; set; }
    }

    public class SubChannelByAccessReturn
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubChanneltoChannel
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
}