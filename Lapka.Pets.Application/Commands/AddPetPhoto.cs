using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class AddPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public List<File> Photos { get; }
        public List<Guid> PhotoIds { get; }

        public AddPetPhoto(Guid petId, List<File> photos, List<Guid> photoIds)
        {
            PetId = petId;
            Photos = photos;
            PhotoIds = photoIds;
        }
    }
}