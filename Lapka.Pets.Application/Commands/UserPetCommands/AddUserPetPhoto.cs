using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class AddUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public List<File> Photos { get; }
        public AddUserPetPhoto(Guid petId, List<File> photos)
        {
            PetId = petId;
            Photos = photos;
        }
    }
}