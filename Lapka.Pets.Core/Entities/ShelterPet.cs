using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class ShelterPet : AggregatePet
    {
        public Address ShelterAddress { get; private set; }
        public string Description { get; private set; }

        public ShelterPet(Guid id, string name, Sex sex, string race, Species species, Guid mainPhotoId,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description, List<Guid> photoIds) : base(id, name, sex, race, species, mainPhotoId, birthDay,
            color, weight, sterilization, photoIds)
        {
            ShelterAddress = shelterAddress;
            Description = description;
        }

        public static ShelterPet Create(Guid id, string name, Sex sex, string race, Species species, Guid photoId,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description, List<Guid> photoIds)
        {
            Validate(name, race, birthDay, color, weight, description);
            ShelterPet pet = new ShelterPet(id, name, sex, race, species, photoId, birthDay, color, weight,
                sterilization, shelterAddress, description, photoIds);

            pet.AddEvent(new PetCreated<ShelterPet>(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay,
            bool sterilization, double weight, string color, Address shelterAddress, string description)
        {
            Update(name, race, species, sex, birthDay, sterilization, weight, color);
            Validate(name, race, birthDay, color, weight, description);

            ShelterAddress = shelterAddress;
            Description = description;

            AddEvent(new PetUpdated<ShelterPet>(this));
        }

        public override void AddPhotos(List<Guid> photoIds)
        {
            base.AddPhotos(photoIds);

            AddEvent(new PetPhotosAdded<ShelterPet>(this, photoIds));
        }

        public override void RemovePhoto(Guid photoId)
        {
            base.RemovePhoto(photoId);
            
            AddEvent(new PetPhotoDeleted<ShelterPet>(this, photoId));

        }
        
        public override void UpdateMainPhoto(Guid photoId)
        {
            base.UpdateMainPhoto(photoId);

            AddEvent(new PetUpdated<ShelterPet>(this));
        }

        public override void Delete()
        {
            AddEvent(new PetDeleted<ShelterPet>(this));
        }

        private static void Validate(string name, string race, DateTime birthDay, string color, double weight,
            string description)
        {
            Validate(name, race, birthDay, color, weight);

            ValidateDescription(description);
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new InvalidDescriptionValueException(description);
        }
    }
}