using Lapka.Pets.Core.Exceptions;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Address
    {
        public string Name { get; }
        public string City { get; }
        public string Street { get; }

        public Address(string name, string city, string street)
        {
            Name = name;
            City = city;
            Street = street;

            Validate();
        }

        private void Validate()
        {
            ValidateName();
            ValidateCity();
            ValidateStreet();
        }

        private void ValidateName()
        {
            if (string.IsNullOrEmpty(Name))
                throw new InvalidAddressNameException(Name);
        }
        
        private void ValidateCity()
        {
            if (string.IsNullOrEmpty(City))
                throw new InvalidCityValueException(City);
            
        }
        
        private void ValidateStreet()
        {
            if (string.IsNullOrEmpty(Street))
                throw new InvalidStreetValueException(Street);
            
        }
    }
}