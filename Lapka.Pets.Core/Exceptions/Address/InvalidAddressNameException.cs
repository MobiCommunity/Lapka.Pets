using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions
{
    public class InvalidAddressNameException : DomainException
    {
        public string Address { get; }
        public InvalidAddressNameException(string address) : base($"Invalid address name value: {address}")
        {
            Address = address;
        }

        public override string Code => "invalid_address_name_value";
    }
}