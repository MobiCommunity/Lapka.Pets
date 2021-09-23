using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Pets.Application.Events.External
{
    [Message("identity")]
    public class ShelterRemoved : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterRemoved(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}