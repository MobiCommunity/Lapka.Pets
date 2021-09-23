using System;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Mongo.Documents
{
    public class LostPetDocument : PetDocument
    {
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LostDate { get; set; }
        public AddressDocument LostAddress { get; set; }
        public LocationDocument LostGeoLocation { get; set; }
        public string Description { get; set; }
    }
}