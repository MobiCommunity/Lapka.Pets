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
        public IEnumerable<File> Photos { get; }

        public AddLostPetPhoto(Guid petId, Guid userId, IEnumerable<File> photos)
        {
            PetId = petId;
            UserId = userId;
            Photos = photos;
        }
    }
}