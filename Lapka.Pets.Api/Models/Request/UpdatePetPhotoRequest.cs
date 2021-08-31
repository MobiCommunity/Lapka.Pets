using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class UpdatePetPhotoRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}