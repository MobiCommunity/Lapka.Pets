using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class DescriptionTooShortException : DomainException
    {
        public string Description { get; }

        public DescriptionTooShortException(string description) : base($"Too short description: {description}") => Description = description;
        public override string Code => "too_short_description";
    }
}