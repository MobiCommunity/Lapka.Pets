using System;

namespace Lapka.Pets.Infrastructure.Mongo.Documents
{
    public class ShelterPetDocument : PetDocument
    {
        public Guid ShelterId { get; set; }
        public string ShelterName { get; set; }
        public AddressDocument ShelterAddress { get; set; }
        public LocationDocument ShelterGeoLocation { get; set; }
        public string Description { get; set; }
    }
}