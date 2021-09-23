using System;
using Convey.CQRS.Events;

namespace Lapka.Pets.Application.Events
{
    public class UserPetRemoved : IEvent
    {
        public Guid Id { get; }

        public UserPetRemoved(Guid id)
        {
            Id = id;
        }
    }
}