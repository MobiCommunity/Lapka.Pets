using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid PhotoId { get; }

        public DeleteUserPetPhoto(Guid petId, Guid photoId)
        {
            PetId = petId;
            PhotoId = photoId;
        }
    }
}