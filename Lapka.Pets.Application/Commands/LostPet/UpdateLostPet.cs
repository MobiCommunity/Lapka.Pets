using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class UpdateLostPet : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Race { get; }
        public Species Species { get; }
        public Sex Sex { get; }
        public DateTime BirthDate { get; }
        public DateTime LostDate { get; }
        public double Weight { get; }
        public string Color { get; }
        public string OwnerName { get; }
        public string PhoneNumber { get; }
        public Address LostAddress { get; }
        public string Description { get; }

        public UpdateLostPet(Guid id, Guid userId, string name, string race, Species species, Sex sex,
            DateTime birthDate, DateTime lostDate, double weight, string color, string ownerName, string phoneNumber,
            Address lostAddress, string description)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            BirthDate = birthDate;
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