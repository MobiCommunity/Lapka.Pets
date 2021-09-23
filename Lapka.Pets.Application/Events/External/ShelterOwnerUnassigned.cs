using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Pets.Application.Events.External
{
    [Message("identity")]
    public class ShelterOwnerUnassigned : IEvent
    {
        public Guid ShelterId { get; }
        public Guid UserId { get; }

        public ShelterOwnerUnassigned(Guid shelterId, Guid userId)
        {
            ShelterId = shelterId;
            UserId = userId;
        }
    }
}