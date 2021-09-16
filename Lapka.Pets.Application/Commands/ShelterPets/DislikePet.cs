using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class DislikePet : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }

        public DislikePet(Guid petId, Guid userId)
        {
            PetId = petId;
            UserId = userId;
        }
    }
}