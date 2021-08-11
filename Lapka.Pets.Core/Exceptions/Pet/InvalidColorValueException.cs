using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidColorValueException : DomainException
    {
        public string Color { get; }

        public InvalidColorValueException(string color) : base($"Invalid color value: {color}") => Color = color;
        public override string Code => "invalid_color_value";
    }
}