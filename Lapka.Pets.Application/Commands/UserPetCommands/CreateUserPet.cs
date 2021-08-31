using System;
using System.Collections.Generic;
using Lapka.Pets.Application.Commands.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreateUserPet : CreatePet
    {
        public Guid UserId { get; }

        public CreateUserPet(Guid id, Guid userId, string name, Sex sex, string race, Species species, PhotoFile photo, DateTime birthDay,
            string color, double weight, bool sterilization, IEnumerable<PhotoFile> photoPaths) :
            base(id, name, sex, race, species, photo, birthDay, color, weight, sterilization, photoPaths)
        {
            UserId = userId;
        }
    }
}