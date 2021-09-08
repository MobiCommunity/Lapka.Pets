using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class UserPet : AggregatePet
    {
        public List<PetEvent> SoonEvents { get; private set; }
        public List<Visit> LastVisits { get; private set; }
        public bool Sterilization { get; private set; }

        public UserPet(Guid id, Guid userId, string name, Sex sex, string race, Species species, Guid photoId,
            DateTime birthDay, string color, double weight, bool sterilization, List<PetEvent> soonEvents,
            List<Visit> lastVisits, List<Guid> photoIds) : base(id, userId, name, sex, race, species, photoId, birthDay, color,
            weight, photoIds)
        {
            SoonEvents = soonEvents;
            LastVisits = lastVisits;
            Sterilization = sterilization;
        }

        public static UserPet Create(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            Guid photoId,
            DateTime birthDay, string color, double weight, bool sterilization, List<Guid> photoIds)
        {
            Validate(name, race, birthDay, color, weight);
            UserPet pet = new UserPet(id, userId, name, sex, race, species, photoId, birthDay, color, weight,
                sterilization, new List<PetEvent>(), new List<Visit>(), photoIds);

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
            LastVisits.Add(visit);
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
            SoonEvents.Add(soonEvent);
            AddEvent(new UserPetUpdated(this));
        }

        public void Update(string name, string race, Species species, Sex sex, DateTime birthDay, double weight, string color, bool sterilization)
        {
            base.Update(name, race, species, sex, birthDay, weight, color);

            Sterilization = sterilization;
            AddEvent(new UserPetUpdated(this));
        }

        public override void AddPhotos(List<Guid> photoIds)
        {
            base.AddPhotos(photoIds);
            
            AddEvent(new UserPetPhotosAdded(this, photoIds));

        }

        public override void RemovePhoto(Guid photoId)
        {
            base.RemovePhoto(photoId);
            
            AddEvent(new UserPetPhotoDeleted(this, photoId));

        }

        public override void UpdateMainPhoto(Guid mainPhotoId)
        {
            base.UpdateMainPhoto(mainPhotoId);
            
            AddEvent(new UserPetUpdated(this));

        }

        public override void Delete()
        {
            AddEvent(new UserPetDeleted(this));
        }
    }
}