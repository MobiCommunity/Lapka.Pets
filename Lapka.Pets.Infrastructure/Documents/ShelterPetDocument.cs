namespace Lapka.Pets.Infrastructure.Documents
{
    public class ShelterPetDocument : PetDocument
    {
        public AddressDocument ShelterAddress { get; set; }
        public string Description { get; set; }
    }
}