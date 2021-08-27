using System;
using System.Collections.Generic;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreateUserPet : CreatePet
    {
        public string UserId { get; }

        public CreateUserPet(Guid id, string userId, string name, Sex sex, string race, Species species, File photo, DateTime birthDay,
            string color, double weight, bool sterilization) : base(id, name, sex, race, species, photo,
            birthDay, color, weight, sterilization)
        {
            UserId = userId;
        }
    }
}