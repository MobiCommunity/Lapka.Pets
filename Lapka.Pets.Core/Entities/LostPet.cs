using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class LostPet : AggregatePet
    {
        public Guid UserId { get; private set; }
        public string OwnerName { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime LostDate { get; private set; }
        public Address LostAddress { get; private set; }
        public string Description { get; private set; }

        public LostPet(Guid id, Guid userId, string name, Sex sex, string race, Species species, Guid mainPhotoId,
            List<Guid> photoIds, DateTime birthDay, string color, double weight, string ownerName,
            string phoneNumber, DateTime lostDate, Address lostAddress, string description) : base(id, name, sex, race,
            species, mainPhotoId, birthDay, color, weight, photoIds)
        {
            UserId = userId;
            OwnerName = ownerName;
            PhoneNumber = phoneNumber;
            LostDate = lostDate;
            LostAddress = lostAddress;
            Description = description;
        }

        public static LostPet Create(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            Guid photoId, List<Guid> photoIds, DateTime birthDay, string color, double weight, string ownerName,
            string phoneNumber, DateTime lostDate, Address lostAddress, string description)
        {
            Validate(name, race, birthDay, color, weight, ownerName, phoneNumber, lostDate, description);
            LostPet pet = new LostPet(id, userId, name, sex, race, species, photoId, photoIds, birthDay, color, weight,
                ownerName, phoneNumber, lostDate, lostAddress, description);

            pet.AddEvent(new PetCreated<LostPet>(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay, double weight,
            string color, string ownerName, string phoneNumber, DateTime lostDate, Address lostAddress,
            string description)
        {
            base.Update(name, race, species, sex, birthDay, weight, color);
            OwnerName = ownerName;
            PhoneNumber = phoneNumber;
            LostDate = lostDate;
            LostAddress = lostAddress;
            Description = description;

            AddEvent(new PetUpdated<LostPet>(this));
        }

        public override void AddPhotos(List<Guid> photoIds)
        {
            base.AddPhotos(photoIds);

            AddEvent(new PetPhotosAdded<LostPet>(this, photoIds));
        }

        public override void RemovePhoto(Guid photoId)
        {
            base.RemovePhoto(photoId);

            AddEvent(new PetPhotoDeleted<LostPet>(this, photoId));
        }

        public override void UpdateMainPhoto(Guid photoId)
        {
            base.UpdateMainPhoto(photoId);

            AddEvent(new PetUpdated<LostPet>(this));
        }

        public override void Delete()
        {
            AddEvent(new PetDeleted<LostPet>(this));
        }

        private static void Validate(string name, string race, DateTime birthDay, string color, double weight,
            string ownerName,
            string phoneNumber, DateTime lostDate, string description)
        {
            Validate(name, race, birthDay, color, weight);

            ValidateOwnerName(ownerName);
            ValidatePhoneNumber(phoneNumber);
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

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            if (!PhoneNumberRegex.IsMatch(phoneNumber))
            {
                throw new InvalidPhoneNumberException(phoneNumber);
            }
        }

        private static void ValidateLostDate(DateTime lostDate)
        {
            if (lostDate > DateTime.Now)
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

        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}