using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class ShelterPet : AggregatePet
    {
        public Guid ShelterId { get; }
        public string ShelterName { get; }
        public Address ShelterAddress { get; private set; }
        public Location ShelterGeoLocation { get; }
        public string Description { get; private set; }
        public bool Sterilization { get; private set; }

        public ShelterPet(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            string mainPhotoPath,
            DateTime birthDay, string color, double weight, bool sterilization, Guid shelterId, string shelterName,
            Address shelterAddress, Location shelterGeoLocation, string description, bool isDeleted = false,
            IEnumerable<string> photoPaths = null) : base(id, userId, name, sex, race, species, mainPhotoPath, birthDay,
            color, weight, isDeleted, photoPaths)
        {
            ShelterId = shelterId;
            ShelterName = shelterName;
            ShelterAddress = shelterAddress;
            ShelterGeoLocation = shelterGeoLocation;
            Description = description;
            Sterilization = sterilization;
        }

        public static ShelterPet Create(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            string mainPhotoId, DateTime birthDay, string color, double weight, bool sterilization, Guid shelterId,
            string shelterName, Address shelterAddress, Location shelterGeoLocation, string description,
            IEnumerable<string> photoIds = null)
        {
            Validate(name, race, birthDay, color, weight, description);
            ShelterPet pet = new ShelterPet(id, userId, name, sex, race, species, mainPhotoId, birthDay, color, weight,
                sterilization, shelterId, shelterName, shelterAddress, shelterGeoLocation, description, false,
                photoIds);

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

        public override void AddPhotos(IEnumerable<string> photoPaths)
        {
            base.AddPhotos(photoPaths);

            AddEvent(new ShelterPetUpdated(this));
        }

        public override void RemovePhotos(IEnumerable<string> photoPaths)
        {
            IEnumerable<string> deletedPhotoPaths = photoPaths as string[] ?? photoPaths.ToArray();

            base.RemovePhotos(deletedPhotoPaths);

            AddEvent(new ShelterPetPhotosDeleted(this, deletedPhotoPaths));
        }

        public void UpdateMainPhoto(string photoId, string oldPhotoPath)
        {
            base.UpdateMainPhoto(photoId);

            AddEvent(new ShelterPetPhotosDeleted(this, new Collection<string> {oldPhotoPath}));
        }

        public override void Delete()
        {
            base.Delete();

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