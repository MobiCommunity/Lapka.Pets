﻿using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteUserPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public Guid PhotoId { get; }

        public DeleteUserPetPhoto(Guid petId, Guid userId, Guid photoId)
        {
            PetId = petId;
            UserId = userId;
            PhotoId = photoId;
        }
    }
}