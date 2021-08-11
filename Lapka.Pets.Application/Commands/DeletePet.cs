using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeletePet : ICommand
    {
        public Guid Id { get; }

        public DeletePet(Guid id)
        {
            Id = id;
        }
    }
}