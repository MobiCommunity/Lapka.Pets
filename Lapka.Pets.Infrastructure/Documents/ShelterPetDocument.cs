using System;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class ShelterPetDocument : PetDocument
    {
        public Guid ShelterId { get; set; }
        public AddressDocument ShelterAddress { get; set; }
        public string Description { get; set; }
    }
}