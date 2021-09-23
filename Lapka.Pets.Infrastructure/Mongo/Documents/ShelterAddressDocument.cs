using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.Mongo.Documents
{
    public class ShelterAddressDocument
    {
        public string Street { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public Location GeoLocation { get; set; }
    }
}