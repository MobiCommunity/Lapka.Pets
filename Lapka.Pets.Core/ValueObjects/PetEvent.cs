using System;

namespace Lapka.Pets.Core.ValueObjects
{
    public class PetEvent
    {
        public DateTime DateOfEvent { get; }
        public string DescriptionOfEvent { get; }

        public PetEvent(DateTime dateOfEvent, string descriptionOfEvent)
        {
            DateOfEvent = dateOfEvent;
            DescriptionOfEvent = descriptionOfEvent;
        }
    }
}