using System;
using System.Collections;
using System.Collections.Generic;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Visit
    {
        public string LocationName { get; }
        public bool IsVisitDone { get; }
        public DateTime VisitDate { get; }
        public string Description { get; }
        public double Weight { get; }
        public IEnumerable<string> MedicalTreatments { get; }

        public Visit(string locationName, bool isVisitDone, DateTime visitDate, string description, double weight,
            IEnumerable<string> medicalTreatments)
        {
            LocationName = locationName;
            IsVisitDone = isVisitDone;
            VisitDate = visitDate;
            Description = description;
            Weight = weight;
            MedicalTreatments = medicalTreatments;
        }
    }
}