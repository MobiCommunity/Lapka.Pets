using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Api.Models.Request
{
    public class DeletePetPhotoRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}