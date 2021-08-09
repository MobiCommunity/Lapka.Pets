using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions
{
    public class InvalidAdressNameException : DomainException
    {
        public string Value { get; }
        public InvalidAdressNameException(string message) : base($"Invalid address name value: {message}")
        {
            Value = message;
        }

        public override string Code => "invalid_address_name_value";
    }
}