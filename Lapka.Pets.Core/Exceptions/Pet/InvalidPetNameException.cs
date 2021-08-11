using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidPetNameException : DomainException
    {
        public string Name { get; }

        public InvalidPetNameException(string name) : base($"Invalid name value: {name}") => Name = name;

        public override string Code => "invalid_pet_name_value";
    }
}