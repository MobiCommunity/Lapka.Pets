using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.LostPets
{
    public class CreateLostPet : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Race { get; }
        public Species Species { get; }
        public Sex Sex { get; }
        public int Age { get; }
        public DateTime LostDate { get; }
        public bool Sterilization { get; }
        public double Weight { get; }
        public string Color { get; }
        public string OwnerName { get; }
        public string PhoneNumber { get; }
        public Address LostAddress { get; }
        public string Description { get; }
        public File MainPhoto { get; }
        public IEnumerable<File> Photos { get; }

        public CreateLostPet(Guid id, Guid userId, string name, string race, Species species, Sex sex,
            int age, DateTime lostDate, bool sterilization, double weight, string color, string ownerName,
            string phoneNumber, Address lostAddress, string description, File mainPhoto, IEnumerable<File> photos)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            Age = age;
            LostDate = lostDate;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
            OwnerName = ownerName;
            PhoneNumber = phoneNumber;
            LostAddress = lostAddress;
            Description = description;
            MainPhoto = mainPhoto;
            Photos = photos;
        }
    }
}