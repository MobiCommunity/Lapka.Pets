using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteShelterPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public string Path { get; }

        public DeleteShelterPetPhoto(Guid petId, string path)
        {
            PetId = petId;
            Path = path;
        }
    }
}