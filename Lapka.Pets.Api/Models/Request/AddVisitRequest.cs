using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Api.Models.Request
{
    public class AddVisitRequest
    {
        [Required]
        public string LocationName { get; set; }
        [Required]
        public bool IsVisitDone { get; set; }
        [Required]
        public DateTime VisitDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public IEnumerable<string> MedicalTreatments { get; set; }
    }
}