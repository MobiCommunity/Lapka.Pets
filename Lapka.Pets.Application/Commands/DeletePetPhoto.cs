using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeletePetPhoto : ICommand
    {
        public Guid PetId { get; }
        public string Path { get; }

        public DeletePetPhoto(Guid petId, string path)
        {
            PetId = petId;
            Path = path;
        }
    }
}