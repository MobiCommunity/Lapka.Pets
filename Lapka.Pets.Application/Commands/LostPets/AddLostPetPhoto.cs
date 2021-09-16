using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.LostPets
{
    public class AddLostPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public List<PhotoFile> Photos { get; }

        public AddLostPetPhoto(Guid petId, Guid userId, List<PhotoFile> photos)
        {
            PetId = petId;
            UserId = userId;
            Photos = photos;
        }
    }
}