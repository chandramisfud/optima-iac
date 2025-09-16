using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities.Dtos
{
    public class EntityReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime CreateOn { get; set; }
    }

    public class EntityInsert
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime CreateOn { get; set; }
    }

    public class EntityBody
    {
        [Required(ErrorMessage = "Empty Entity name")]
        [MaxLength(20, ErrorMessage = "Up to 20 chars")]
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
    }
}