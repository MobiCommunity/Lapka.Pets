using System;
using Convey.CQRS.Events;

namespace Lapka.Pets.Application.Events
{
    public class ShelterPetRemoved : IEvent
    {
        public Guid Id { get; }

        public ShelterPetRemoved(Guid id)
        {
            Id = id;
        }
    }
}