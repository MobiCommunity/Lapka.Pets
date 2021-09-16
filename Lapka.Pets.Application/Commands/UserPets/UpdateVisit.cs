using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.UserPets
{
    public class UpdateVisit : ICommand
    {
        public Guid UserId { get; }
        public Guid PetId { get; }
        public Visit UpdatedVisit { get; }

        public UpdateVisit(Guid userId, Guid petId, Visit updatedVisit)
        {
            UserId = userId;
            PetId = petId;
            UpdatedVisit = updatedVisit;
        }
    }
}