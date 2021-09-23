using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class UserPet : AggregatePet
    {
        private ISet<PetEvent> _soonEvents = new HashSet<PetEvent>();
        private ISet<Visit> _lastVisits = new HashSet<Visit>();

        public IEnumerable<PetEvent> SoonEvents
        {
            get => _soonEvents;
            private set => _soonEvents = new HashSet<PetEvent>(value);
        }

        public IEnumerable<Visit> LastVisits
        {
            get => _lastVisits;
            private set => _lastVisits = new HashSet<Visit>(value);
        }

        public bool Sterilization { get; private set; }

        public UserPet(Guid id, Guid userId, string name, Sex sex, string race, Species species, string photoPath,
            DateTime birthDay, string color, double weight, bool sterilization, IEnumerable<PetEvent> soonEvents = null,
            IEnumerable<Visit> lastVisits = null, bool isDeleted = false, IEnumerable<string> photoPaths = null) : base(
            id, userId, name, sex, race, species, photoPath, birthDay, color, weight, isDeleted, photoPaths)
        {
            SoonEvents = soonEvents ?? Enumerable.Empty<PetEvent>();
            LastVisits = lastVisits ?? Enumerable.Empty<Visit>();
            Sterilization = sterilization;
        }

        public static UserPet Create(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            string photoPath, DateTime birthDay, string color, double weight, bool sterilization,
            IEnumerable<string> photoIds = null)
        {
            Validate(name, race, birthDay, color, weight);
            UserPet pet = new UserPet(id, userId, name, sex, race, species, photoPath, birthDay, color, weight,
                sterilization, isDeleted: false, photoPaths: photoIds);

            pet.AddEvent(new UserPetCreated(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay,
            bool sterilization, double weight, string color)
        {
            base.Update(name, race, species, sex, birthDay, weight, color);
            Sterilization = sterilization;

            Validate(name, race, birthDay, color, weight);

            AddEvent(new UserPetUpdated(this));
        }

        public void AddLastVisit(Visit visit)
        {
            _lastVisits.Add(visit);
            AddEvent(new UserPetUpdated(this));
        }

        public void UpdateLastVisit(Visit visitToUpdate, Visit updatedVisit)
        {
            visitToUpdate.Update(updatedVisit.LocationName, updatedVisit.IsVisitDone, updatedVisit.VisitDate,
                updatedVisit.Description, updatedVisit.Weight, updatedVisit.MedicalTreatments);

            AddEvent(new UserPetUpdated(this));
        }

        public void AddSoonEvent(PetEvent soonEvent)
        {
            _soonEvents.Add(soonEvent);
            AddEvent(new UserPetUpdated(this));
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay, double weight,
            string color, bool sterilization)
        {
            base.Update(name, race, species, sex, birthDay, weight, color);

            Sterilization = sterilization;
            AddEvent(new UserPetUpdated(this));
        }

        public override void AddPhotos(IEnumerable<string> photoPaths)
        {
            base.AddPhotos(photoPaths);

            AddEvent(new UserPetUpdated(this));
        }

        public override void RemovePhotos(IEnumerable<string> photoPaths)
        {
            IEnumerable<string> deletedPhotoPaths = photoPaths as string[] ?? photoPaths.ToArray();

            base.RemovePhotos(deletedPhotoPaths);

            AddEvent(new UserPetPhotosDeleted(this, deletedPhotoPaths));
        }

        public void UpdateMainPhoto(string mainPhotoId, string oldPhotoPath)
        {
            base.UpdateMainPhoto(mainPhotoId);

            AddEvent(new UserPetPhotosDeleted(this, new Collection<string> {oldPhotoPath}));
        }

        public override void Delete()
        {
            base.Delete();

            AddEvent(new UserPetDeleted(this));
        }
    }
}