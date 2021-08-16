using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class AddPetPhotoRequest
    {
        public List<IFormFile> Photo { get; set; }
    }
}