using System;

namespace Lapka.Pets.Core.ValueObjects
{
    public class PetEvent : IEquatable<PetEvent>
    {
        public Guid Id { get; }
        public DateTime DateOfEvent { get; }
        public string DescriptionOfEvent { get; }

        public PetEvent(Guid id, DateTime dateOfEvent, string descriptionOfEvent)
        {
            Id = id;
            DateOfEvent = dateOfEvent;
            DescriptionOfEvent = descriptionOfEvent;
        }

        public bool Equals(PetEvent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PetEvent) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}