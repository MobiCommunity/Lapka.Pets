using System;
using System.Collections.Generic;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class LostPetDocument : PetDocument
    {
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LostDate { get; set; }
        public AddressDocument LostAddress { get; set; }
        public string Description { get; set; }
    }
}