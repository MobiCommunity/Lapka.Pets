using System;
using System.Collections.Generic;
using Lapka.Pets.Application.Commands.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class CreateShelterPet : CreatePet
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public string Description { get; }
        public bool Sterilization { get; }

        public CreateShelterPet(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            File photo, DateTime birthDay, string color, double weight, bool sterilization, string description,
            Guid shelterId, IEnumerable<File> photos = null) : base(id, name, sex, race, species, photo, birthDay, color,
            weight, photos)
        {
            UserId = userId;
            Sterilization = sterilization;
            Description = description;
            ShelterId = shelterId;
        }
    }
}