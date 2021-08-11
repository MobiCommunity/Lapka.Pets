using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Location
{
    public class LongitudeTooLowException : DomainException
    {
        public string Longitude { get; }
        public LongitudeTooLowException(string longitude) : base($"Longitude too low: {longitude}")
        {
            Longitude = longitude;
        }

        public override string Code => "longitude_too_low";
    }
}