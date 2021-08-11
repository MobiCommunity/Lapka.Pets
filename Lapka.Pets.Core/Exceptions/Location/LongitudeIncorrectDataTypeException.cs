using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Location
{
    public class LongitudeIncorrectDataTypeException : DomainException
    {
        public string Longitude { get; }
        public LongitudeIncorrectDataTypeException(string longitude) : base($"Invalid data type of longitude: {longitude}")
        {
            Longitude = longitude;
        }

        public override string Code => "invalid_longitude_data_type";
    }
}