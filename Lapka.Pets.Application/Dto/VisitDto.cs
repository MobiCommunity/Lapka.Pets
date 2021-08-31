using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class VisitDto
    {
        [Required]
        public Guid Id { get; set; }
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