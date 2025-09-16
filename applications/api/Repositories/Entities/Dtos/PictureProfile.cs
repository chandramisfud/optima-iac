using Microsoft.AspNetCore.Http;

namespace Repositories.Entities.Dtos
{
    public class PictureProfileBody
    {
//        public string UserEmail { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}