using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public string Path { get; }

        public DeleteUserPetPhoto(Guid petId, string path)
        {
            PetId = petId;
            Path = path;
        }
    }
}