using System;
using System.Collections;
using System.Collections.Generic;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Visit
    {
        public Guid Id { get; private set; }
        public string LocationName { get; private set; }
        public bool IsVisitDone { get; private set; }
        public DateTime VisitDate { get; private set; }
        public string Description { get; private set; }
        public double Weight { get; private set; }
        public IEnumerable<string> MedicalTreatments { get; private set; }

        public Visit(Guid id, string locationName, bool isVisitDone, DateTime visitDate, string description, double weight,
            IEnumerable<string> medicalTreatments)
        {
            Id = id;
            LocationName = locationName;
            IsVisitDone = isVisitDone;
            VisitDate = visitDate;
            Description = description;
            Weight = weight;
            MedicalTreatments = medicalTreatments;
        }

        public void Update(string locationName, bool isVisitDone, DateTime visitDate, string description, double weight,
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