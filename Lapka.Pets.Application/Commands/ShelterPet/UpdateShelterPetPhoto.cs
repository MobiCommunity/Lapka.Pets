using System;
using System.IO;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Application.Commands
{
    public class UpdateShelterPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public PhotoFile Photo { get; }
        public UpdateShelterPetPhoto(Guid petId, Guid userId, PhotoFile photo)
        {
            PetId = petId;
            UserId = userId;
            Photo = photo;
        }
    }
}