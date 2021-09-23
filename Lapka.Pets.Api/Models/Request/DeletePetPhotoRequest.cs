using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Api.Models.Request
{
    public class DeletePetPhotoRequest
    {
        [Required]
        public IEnumerable<string> Path { get; set; }
    }
}