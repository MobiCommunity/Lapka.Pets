using System;
using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class InvalidLostDateValueException : DomainException
    {
        public DateTime LostDate { get; }

        public InvalidLostDateValueException(DateTime lostDate) : base($"Invalid lost date value: {lostDate}") =>
            LostDate = lostDate;

        public override string Code => "invalid_lost_date_value";
    }
}