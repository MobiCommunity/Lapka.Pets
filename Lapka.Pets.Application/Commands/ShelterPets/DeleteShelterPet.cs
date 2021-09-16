using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class DeleteShelterPet : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public DeleteShelterPet(Guid id, Guid userId)
        {
            UserId = userId;
            Id = id;
        }
    }
}