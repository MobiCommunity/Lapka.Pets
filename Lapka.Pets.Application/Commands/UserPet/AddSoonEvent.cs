using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class AddSoonEvent : ICommand
    {
        public Guid UserId { get; }
        public Guid PetId { get; }
        public PetEvent SoonEvent { get; }

        public AddSoonEvent(Guid userId, Guid petId, PetEvent soonEvent)
        {
            UserId = userId;
            PetId = petId;
            SoonEvent = soonEvent;
        }
    }
}