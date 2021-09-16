using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.LostPets
{
    public class DeleteLostPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public Guid PhotoId { get; }

        public DeleteLostPetPhoto(Guid petId, Guid userId, Guid photoId)
        {
            PetId = petId;
            UserId = userId;
            PhotoId = photoId;
        }
    }
}