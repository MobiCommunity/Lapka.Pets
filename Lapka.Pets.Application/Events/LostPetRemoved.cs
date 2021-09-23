using System;
using Convey.CQRS.Events;

namespace Lapka.Pets.Application.Events
{
    public class LostPetRemoved : IEvent
    {
        public Guid Id { get; }

        public LostPetRemoved(Guid id)
        {
            Id = id;
        }
    }
}