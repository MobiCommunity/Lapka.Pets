using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidDescriptionValueException : DomainException
    {
        public string Description { get; }

        public InvalidDescriptionValueException(string description) : base($"Invalid description value: {description}") => Description = description;
        public override string Code => "invalid_description_value";
    }
}