using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class ShelterPet : AggregatePet
    {
        public Guid ShelterId { get; private set; }
        public Address ShelterAddress { get; private set; }
        public string Description { get; private set; }
        public bool Sterilization { get; private set; }
        
        public ShelterPet(Guid id, Guid userId, string name, Sex sex, string race, Species species, Guid mainPhotoId,
            DateTime birthDay, string color, double weight, bool sterilization, Guid shelterId, Address shelterAddress,
            string description, List<Guid> photoIds) : base(id, userId, name, sex, race, species, mainPhotoId, birthDay,
            color, weight, photoIds)
        {
            ShelterAddress = shelterAddress;
            Description = description;
            Sterilization = sterilization;
            ShelterId = shelterId;
        }

        public static ShelterPet Create(Guid id, Guid userId, string name, Sex sex, string race, Species species, Guid photoId,
            DateTime birthDay, string color, double weight, bool sterilization, Guid shelterId, Address shelterAddress,
            string description, List<Guid> photoIds)
        {
            Validate(name, race, birthDay, color, weight, description);
            ShelterPet pet = new ShelterPet(id, userId, name, sex, race, species, photoId, birthDay, color, weight,
                sterilization, shelterId, shelterAddress, description, photoIds);

            pet.AddEvent(new ShelterPetCreated(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay,
            bool sterilization, double weight, string color, Address shelterAddress, string description)
        {
            Update(name, race, species, sex, birthDay, weight, color);
            Validate(name, race, birthDay, color, weight, description);

            ShelterAddress = shelterAddress;
            Description = description;
            Sterilization = sterilization;

            AddEvent(new ShelterPetUpdated(this));
        }
        public override void AddPhotos(List<Guid> photoIds)
        {
            base.AddPhotos(photoIds);

            AddEvent(new ShelterPetPhotosAdded(this, photoIds));
        }

        public override void RemovePhoto(Guid photoId)
        {
            base.RemovePhoto(photoId);
            
            AddEvent(new ShelterPetPhotoDeleted(this, photoId));
        }
        
        public override void UpdateMainPhoto(Guid photoId)
        {
            base.UpdateMainPhoto(photoId);

            AddEvent(new ShelterPetUpdated(this));
        }

        public override void Delete()
        {
            AddEvent(new ShelterPetDeleted(this));
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