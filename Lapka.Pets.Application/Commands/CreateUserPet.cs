using System;
using System.Collections.Generic;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreateUserPet : CreatePet
    {
        public IEnumerable<PetEvent> PetEvents { get; }
        public IEnumerable<Visit> Visits { get; }

        public CreateUserPet(Guid id, string name, Sex sex, string race, Species species, File photo, DateTime birthDay,
            string color, double weight, bool sterilization, IEnumerable<PetEvent> petEvents, IEnumerable<Visit> visits)
            : base(id, name, sex, race, species, photo, birthDay, color, weight, sterilization)
        {
            PetEvents = petEvents;
            Visits = visits;
        }
    }
}