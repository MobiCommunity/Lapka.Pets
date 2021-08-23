using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Application.Dto
{
    public class LocationDto
    {
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }        

}