using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class AddPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public File Photo { get; }
        public Guid PhotoId { get; }

        public AddPetPhoto(Guid petId, File photo, Guid photoId)
        {
            PetId = petId;
            Photo = photo;
            PhotoId = photoId;
        }
    }
}