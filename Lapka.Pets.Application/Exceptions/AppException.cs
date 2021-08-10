using System;

namespace Lapka.Pets.Application.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract string Code { get; }

        protected AppException(string race) : base(race)
        {
        }
    }

}