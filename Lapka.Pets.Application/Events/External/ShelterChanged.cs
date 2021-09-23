using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Pets.Application.Events.External
{
    [Message("identity")]
    public class ShelterChanged : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterChanged(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}