namespace Repositories.Entities.Dtos
{

    public class DashboardSearch
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? tipe { get; set; }
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? categoryLongDesc { get; set; }
    }

}
