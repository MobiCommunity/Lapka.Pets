using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class AddUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public List<PhotoFile> Photos { get; }
        public AddUserPetPhoto(Guid petId, List<PhotoFile> photos)
        {
            PetId = petId;
            Photos = photos;
        }
    }
}