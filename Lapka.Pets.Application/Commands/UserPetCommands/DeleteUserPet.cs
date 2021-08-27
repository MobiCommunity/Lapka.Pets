using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteUserPet : ICommand
    {
        public string UserId { get; }
        public Guid PetId { get; }

        public DeleteUserPet(string userId, Guid petId)
        {
            UserId = userId;
            PetId = petId;
        }
    }
}