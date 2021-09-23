namespace Lapka.Pets.Core.ValueObjects
{
    public class ShelterBasicInfo
    {
        public string Name { get; }
        public Location Location { get; }
        public Address Address { get; }

        public ShelterBasicInfo(string name, Location location, Address address)
        {
            Name = name;
            Location = location;
            Address = address;
        }
    }
}