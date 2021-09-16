using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.UserPets
{
    public class AddUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public List<PhotoFile> Photos { get; }

        public AddUserPetPhoto(Guid petId, Guid userId, List<PhotoFile> photos)
        {
            PetId = petId;
            UserId = userId;
            Photos = photos;
        }
    }
}