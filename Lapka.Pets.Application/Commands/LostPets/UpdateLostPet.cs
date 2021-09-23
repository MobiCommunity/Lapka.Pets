using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.LostPets
{
    public class UpdateLostPet : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Race { get; }
        public Species Species { get; }
        public Sex Sex { get; }
        public int Age { get; }
        public DateTime LostDate { get; }
        public double Weight { get; }
        public string Color { get; }
        public string OwnerName { get; }
        public PhoneNumber PhoneNumber { get; }
        public Address LostAddress { get; }
        public string Description { get; }

        public UpdateLostPet(Guid id, Guid userId, string name, string race, Species species, Sex sex,
            int age, DateTime lostDate, double weight, string color, string ownerName, PhoneNumber phoneNumber,
            Address lostAddress, string description)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            Age = age;
            LostDate = lostDate;
            Weight = weight;
            Color = color;
            OwnerName = ownerName;
            PhoneNumber = phoneNumber;
            LostAddress = lostAddress;
            Description = description;
        }
    }
}