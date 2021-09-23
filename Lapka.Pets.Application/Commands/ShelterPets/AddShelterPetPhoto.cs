﻿using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class AddShelterPetPhoto : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }
        public IEnumerable<File> Photos { get; }
        public AddShelterPetPhoto(Guid petId, Guid userId, IEnumerable<File> photos)
        {
            UserId = userId;
            PetId = petId;
            Photos = photos;
        }
    }
}