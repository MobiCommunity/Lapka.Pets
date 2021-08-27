using System;
using System.Collections;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class UserPet : AggregatePet
    {
        public IEnumerable<PetEvent> SoonEvents { get; private set; }
        public IEnumerable<Visit> LastVisits { get; private set; }

        public UserPet(Guid id, string name, Sex sex, string race, Species species, string photoPath, DateTime birthDay,
            string color, double weight, bool sterilization, IEnumerable<PetEvent> soonEvents,
            IEnumerable<Visit> lastVisits) : base(id, name, sex, race, species, photoPath, birthDay, color, weight,
            sterilization)
        {
            SoonEvents = soonEvents;
            LastVisits = lastVisits;
        }

        public static UserPet Create(Guid id, string name, Sex sex, string race, Species species, string photoPath,
            DateTime birthDay, string color, double weight, bool sterilization)
        {
            Validate(name, race, birthDay, color, weight);
            UserPet pet = new UserPet(id, name, sex, race, species, photoPath, birthDay, color, weight,
                sterilization, new List<PetEvent>(), new List<Visit>());

            pet.AddEvent(new PetCreated<UserPet>(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, string photoPath, Sex sex, DateTime birthDay,
            bool sterilization, double weight, string color)
        {
            Update(name, race, species, photoPath, sex, birthDay, sterilization, weight, color);
            Validate(name, race, birthDay, color, weight);

            AddEvent(new PetUpdated<UserPet>(this));
        }

        public override void Delete()
        {
            AddEvent(new PetDeleted<UserPet>(this));
        }
    }
}