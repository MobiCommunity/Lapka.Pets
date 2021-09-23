using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Api.Models
{
    public class PhoneNumberModel
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}