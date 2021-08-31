using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteUserPet : ICommand
    {
        public Guid UserId { get; }
        public Guid PetId { get; }

        public DeleteUserPet(Guid userId, Guid petId)
        {
            UserId = userId;
            PetId = petId;
        }
    }
}