using System.Text;
using System.Text.Json.Serialization;

namespace Repositories.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum enDataSortDirection
    {
        asc,
        desc
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum enShowDeleted
    {
        hide,
        show
    }
}
