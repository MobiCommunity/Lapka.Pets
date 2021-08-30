using System;
using System.IO;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Application.Commands
{
    public class UpdateUserPetPhoto : ICommand
    {
        public Guid Id { get; }
        public PhotoFile Photo { get; }

        public UpdateUserPetPhoto(Guid id, PhotoFile photo)
        {
            Id = id;
            Photo = photo;
        }
    }
}