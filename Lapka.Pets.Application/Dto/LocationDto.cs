using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Application.Dto
{
    public class LocationDto
    {
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
    }        

}