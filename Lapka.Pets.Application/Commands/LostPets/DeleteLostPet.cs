using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.LostPets
{
    public class DeleteLostPet : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }

        public DeleteLostPet(Guid petId, Guid userId)
        {
            PetId = petId;
            UserId = userId;
        }
    }
}