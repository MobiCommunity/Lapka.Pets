using System;
using System.Collections.Generic;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class VisitDocument
    {
        public Guid Id { get; set; }
        public string LocationName { get; set; }
        public bool IsVisitDone { get; set; }
        public DateTime VisitDate { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public IEnumerable<string> MedicalTreatments { get; set; }
    }
}