using System;
using Convey.CQRS.Events;

namespace Lapka.Pets.Application.Events
{
    public class ShelterPetMade : IEvent
    {
        public Guid Id { get; }

        public ShelterPetMade(Guid id)
        {
            Id = id;
        }
    }
}