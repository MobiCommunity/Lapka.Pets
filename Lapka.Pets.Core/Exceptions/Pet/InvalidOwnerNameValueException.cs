using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidOwnerNameValueException : DomainException
    {
        public string OwnerName { get; }

        public InvalidOwnerNameValueException(string ownerName) : base($"Invalid Owner name value: {ownerName}") => OwnerName = ownerName;
        public override string Code => "invalid_owner_name_value";
    }
}