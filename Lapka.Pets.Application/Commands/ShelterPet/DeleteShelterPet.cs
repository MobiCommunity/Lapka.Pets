using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteShelterPet : ICommand
    {
        public Guid Id { get; }

        public DeleteShelterPet(Guid id)
        {
            Id = id;
        }
    }
}