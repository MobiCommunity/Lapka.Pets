using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class DeleteShelterPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public IEnumerable<string> PhotoPaths { get; }

        public DeleteShelterPetPhoto(Guid petId, Guid userId, IEnumerable<string> photoPaths)
        {
            UserId = userId;
            PetId = petId;
            PhotoPaths = photoPaths;
        }
    }
}