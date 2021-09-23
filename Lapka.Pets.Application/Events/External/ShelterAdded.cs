using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Pets.Application.Events.External
{
    [Message("identity")]
    public class ShelterAdded : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterAdded(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}