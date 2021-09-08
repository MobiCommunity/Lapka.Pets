using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class ReportStrayPetRequest
    {
        [Required]
        public LocationModel Location { get; set; }
        [Required] 
        public List<IFormFile> Photos { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ReporterName { get; set; }
        [Required]
        public string ReporterPhoneNumber { get; set; }
        
    }
}