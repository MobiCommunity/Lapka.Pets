using System;
using System.Collections;
using System.Collections.Generic;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Visit : IEquatable<Visit>
    {
        private ISet<string> _medicalTreatments = new HashSet<string>();
        public Guid Id { get; }
        public string LocationName { get; private set; }
        public bool IsVisitDone { get; private set; }
        public DateTime VisitDate { get; private set; }
        public string Description { get; private set; }
        public double Weight { get; private set; }

        public IEnumerable<string> MedicalTreatments
        {
            get => _medicalTreatments;
            private set => _medicalTreatments = new HashSet<string>(value);
        }

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

        public bool Equals(Visit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Visit) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}