using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions
{
    public class InvalidStreetValueException : DomainException
    {
        public string Street { get; }
        public InvalidStreetValueException(string street) : base($"Invalid value of street address: {street}")
        {
            Street = street;
        }

        public override string Code => "invalid_street_address";
    }
}