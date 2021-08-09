namespace Lapka.Pets.Core.ValueObject
{
    public class ShelterAddress
    {
        public string Street { get; }
        public string Name { get; }
        public string City { get; }
        public Location GeoLocation { get; }

        public ShelterAddress(string street, string zipCode, string city, Location geoLocation)
        {
            Street = street;
            Name = zipCode;
            City = city;
            GeoLocation = geoLocation;
        }
    }
}