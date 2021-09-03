using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public abstract class AggregatePet : AggregateRoot
    {
        private const double MinimumWeight = 0;

        public Guid UserId { get; }
        public string Name { get; private set; }
        public Species Species { get; private set; }
        public Sex Sex { get; private set; }
        public Guid MainPhotoId { get; private set; }
        public List<Guid> PhotoIds { get; private set; }
        public string Race { get; private set; }
        public DateTime BirthDay { get; private set; }
        public string Color { get; private set; }
        public double Weight { get; private set; }

        protected AggregatePet(Guid id, Guid userId, string name, Sex sex, string race, Species species, Guid mainPhotoId,
            DateTime birthDay, string color, double weight, List<Guid> photoIds)
        {
            Id = new AggregateId(id);
            UserId = userId;
            Name = name;
            Sex = sex;
            Race = race;
            Species = species;
            MainPhotoId = mainPhotoId;
            PhotoIds = photoIds;
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
        }

        public virtual void Update(string name, string race, Species species, Sex sex, DateTime birthDay, double weight,
            string color)
        {
            Validate(name, race, birthDay, color, weight);

            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            BirthDay = birthDay;
            Weight = weight;
            Color = color;
        }

        public virtual void UpdateMainPhoto(Guid mainPhotoId)
        {
            MainPhotoId = mainPhotoId;
        }

        public virtual void AddPhotos(List<Guid> photoIds)
        {
            foreach (Guid path in photoIds)
            {
                PhotoIds.Add(path);
            }
        }

        public virtual void RemovePhoto(Guid photoId)
        {
            PhotoIds.Remove(photoId);
        }

        public abstract void Delete();

        protected static void Validate(string name, string race, DateTime birthDay, string color, double weight)
        {
            ValidateName(name);
            ValidateRace(race);
            ValidateBirthDay(birthDay);
            ValidateColor(color);
            ValidateWeight(weight);
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidPetNameException(name);
        }

        private static void ValidateRace(string race)
        {
            if (string.IsNullOrEmpty(race))
                throw new InvalidRaceValueException(race);
        }

        private static void ValidateBirthDay(DateTime birthDate)
        {
            if (birthDate >= DateTime.Now)
                throw new InvalidBirthDayValueException(birthDate);
        }

        private static void ValidateColor(string color)
        {
            if (string.IsNullOrEmpty(color))
                throw new InvalidColorValueException(color);
        }

        private static void ValidateWeight(double weight)
        {
            if (weight <= MinimumWeight)
                throw new WeightBelowMinimumValueException(weight);
        }
    }
}