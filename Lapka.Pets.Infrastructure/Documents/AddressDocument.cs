namespace Lapka.Pets.Infrastructure.Documents
{
    public class AddressDocument
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public LocationDocument GeoLocation { get; set; }
    }
}