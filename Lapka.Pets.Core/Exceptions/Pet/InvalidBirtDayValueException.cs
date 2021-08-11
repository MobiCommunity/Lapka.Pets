using System;
using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidBirtDayValueException : DomainException
    {
        public DateTime BirthDay { get; }

        public InvalidBirtDayValueException(DateTime birthTime) : base($"Invalid birth day value: {birthTime}") =>
            BirthDay = birthTime;

        public override string Code => "invalid_birth_day_value";
    }
}