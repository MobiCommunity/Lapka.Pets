using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models
{
    public class LocationModel
    {
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
    }
}