﻿using System;
using System.IO;
using Convey.CQRS.Commands;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Application.Commands
{
    public class UpdatePetPhoto : ICommand
    {
        public Guid Id { get; }
        public File Photo { get; }
        public Guid PhotoId { get; }

        public UpdatePetPhoto(Guid id, File photo, Guid photoId)
        {
            Id = id;
            Photo = photo;
            PhotoId = photoId;
        }
    }
}