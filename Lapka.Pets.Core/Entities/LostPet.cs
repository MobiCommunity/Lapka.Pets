using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Losts;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.VisualBasic;

namespace Lapka.Pets.Core.Entities
{
    public class LostPet : AggregatePet
    {
        public string OwnerName { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public DateTime LostDate { get; private set; }
        public Address LostAddress { get; private set; }
        public string Description { get; private set; }


        public LostPet(Guid id, Guid userId, string name, Sex sex, string race, Species species, string mainPhotoPath,
            DateTime birthDay, string color, double weight, string ownerName, PhoneNumber phoneNumber, DateTime lostDate,
            Address lostAddress, string description, bool isDeleted = false, IEnumerable<string> photoPaths = null) : base(id, userId,
            name, sex, race, species, mainPhotoPath, birthDay, color, weight, isDeleted, photoPaths)
        {
            OwnerName = ownerName;
            PhoneNumber = phoneNumber;
            LostDate = lostDate;
            LostAddress = lostAddress;
            Description = description;
        }

        public static LostPet Create(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            string photoId, DateTime birthDay, string color, double weight, string ownerName, PhoneNumber phoneNumber,
            DateTime lostDate, Address lostAddress, string description, IEnumerable<string> photoIds = null)
        {
            Validate(name, race, birthDay, color, weight, ownerName, lostDate, description);
            LostPet pet = new LostPet(id, userId, name, sex, race, species, photoId, birthDay, color, weight,
                ownerName, phoneNumber, lostDate, lostAddress, description, false, photoIds);

            pet.AddEvent(new LostPetCreated(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay, double weight,
            string color, string ownerName, PhoneNumber phoneNumber, DateTime lostDate, Address lostAddress,
            string description)
        {
            base.Update(name, race, species, sex, birthDay, weight, color);
            OwnerName = ownerName;
            PhoneNumber = phoneNumber;
            LostDate = lostDate;
            LostAddress = lostAddress;
            Description = description;

            AddEvent(new LostPetUpdated(this));
        }

        public override void AddPhotos(IEnumerable<string> photoPaths)
        {
            base.AddPhotos(photoPaths);

            AddEvent(new LostPetUpdated(this));
        }

        public override void RemovePhotos(IEnumerable<string> photoPaths)
        {
            IEnumerable<string> deletedPhotoPaths = photoPaths as string[] ?? photoPaths.ToArray();
            
            base.RemovePhotos(deletedPhotoPaths);

            AddEvent(new LostPetPhotosDeleted(this, deletedPhotoPaths));
        }

        public void UpdateMainPhoto(string photoId, string oldPhotoPath)
        {
            base.UpdateMainPhoto(photoId);

            AddEvent(new LostPetPhotosDeleted(this, new Collection<string>{oldPhotoPath}));
        }

        public override void Delete()
        {
            base.Delete();
            
            AddEvent(new LostPetDeleted(this));
        }

        private static void Validate(string name, string race, DateTime birthDay, string color, double weight,
            string ownerName, DateTime lostDate, string description)
        {
            Validate(name, race, birthDay, color, weight);

            ValidateOwnerName(ownerName);
            ValidateLostDate(lostDate);
            ValidateDescription(description);
        }

        private static void ValidateOwnerName(string ownerName)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                throw new InvalidOwnerNameValueException(ownerName);
            }
        }

        private static void ValidateLostDate(DateTime lostDate)
        {
            if (lostDate > DateTime.UtcNow)
            {
                throw new InvalidLostDateValueException(lostDate);
            }
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidDescriptionValueException(description);
            }

            if (description.Length < 20)
            {
                throw new DescriptionTooShortException(description);
            }
        }
    }
}