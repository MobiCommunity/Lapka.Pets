using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.UserPets
{
    public class DeleteUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public IEnumerable<string> PhotoPaths { get; }

        public DeleteUserPetPhoto(Guid petId, Guid userId, IEnumerable<string> photoPaths)
        {
            PetId = petId;
            UserId = userId;
            PhotoPaths = photoPaths;
        }
    }
}