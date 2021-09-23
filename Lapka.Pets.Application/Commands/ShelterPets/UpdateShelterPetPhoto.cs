using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class UpdateShelterPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public File Photo { get; }
        public UpdateShelterPetPhoto(Guid petId, Guid userId, File photo)
        {
            PetId = petId;
            UserId = userId;
            Photo = photo;
        }
    }
}