using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions
{
    public class InvalidCityValueException : DomainException
    {
        public string Value { get; set; }
        public InvalidCityValueException(string value) : base($"Invalid name of city: {value}")
        {
            Value = value;
        }

        public override string Code => "invalid_city_name";
    }
}