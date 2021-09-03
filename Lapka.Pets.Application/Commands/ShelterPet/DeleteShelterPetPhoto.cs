using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteShelterPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public Guid PhotoId { get; }

        public DeleteShelterPetPhoto(Guid petId, Guid userId, Guid photoId)
        {
            UserId = userId;
            PetId = petId;
            PhotoId = photoId;
        }
    }
}