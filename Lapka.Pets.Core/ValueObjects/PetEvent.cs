using System;

namespace Lapka.Pets.Core.ValueObjects
{
    public class PetEvent
    {
        public Guid Id { get; set; }
        public DateTime DateOfEvent { get; }
        public string DescriptionOfEvent { get; }

        public PetEvent(Guid id, DateTime dateOfEvent, string descriptionOfEvent)
        {
            Id = id;
            DateOfEvent = dateOfEvent;
            DescriptionOfEvent = descriptionOfEvent;
        }
    }
}