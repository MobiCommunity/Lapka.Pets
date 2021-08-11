using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidRaceValueException : DomainException
    {
        public string Race { get; }

        public InvalidRaceValueException(string race) : base($"Invalid race value: {race}") => Race = race;

        public override string Code => "invalid_race_value";
    }
}