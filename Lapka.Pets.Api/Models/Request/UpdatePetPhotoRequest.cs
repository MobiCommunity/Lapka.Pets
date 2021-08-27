using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class UpdatePetPhotoRequest
    {
        public IFormFile File { get; set; }
    }
}