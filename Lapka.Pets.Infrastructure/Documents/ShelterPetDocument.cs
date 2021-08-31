namespace Lapka.Pets.Infrastructure.Documents
{
    public class PetShelterDocument : PetDocument
    {
        public AddressDocument ShelterAddress { get; set; }
        public string Description { get; set; }
    }
}