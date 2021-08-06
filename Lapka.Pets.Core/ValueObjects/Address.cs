namespace Lapka.Pets.Core.ValueObjects
{
    public class Address
    {
        public string Name { get; }
        public string City { get; }
        public string Street { get; }
        public Location GeoLocation { get; }

        public Address(string name, string city, string street, Location geoLocation)
        {
            Name = name;
            City = city;
            Street = street;
            GeoLocation = geoLocation;
        }
    }
}