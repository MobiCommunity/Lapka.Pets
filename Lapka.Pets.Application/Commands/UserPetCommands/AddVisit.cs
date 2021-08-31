using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class AddVisit : ICommand
    {
        public Guid UserId { get; }
        public Guid PetId { get; }
        public Visit Visit { get; }

        public AddVisit(Guid userId, Guid petId, Visit visit)
        {
            UserId = userId;
            PetId = petId;
            Visit = visit;
        }
    }
}