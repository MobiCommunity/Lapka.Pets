using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class AddPetPhotoRequest
    {
        [Required]
        public IEnumerable<IFormFile> Photos { get; set; }
    }
}