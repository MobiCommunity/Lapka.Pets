using Lapka.Pets.Core.Exceptions.Location;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Location
    {
        private const int MaxLongitudeValue = 180;
        private const int MinLongitudeValue = -180;
        private const int MaxLatitudeValue = 90;
        private const int MinLatitudeValue = -90;
        public string Latitude { get; }
        public string Longitude { get; }
        public Location(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;

            ValidateLongitude();
            ValidateLatitude();
        }
        
        private void ValidateLongitude()
        {
            if (string.IsNullOrWhiteSpace(Longitude))
            {
                throw new InvalidLongitudeValueException(Longitude);
            }
                
            if (double.Parse(Longitude) <= MinLongitudeValue)
            {
                throw new LongitudeTooLowException(Longitude);
            }

            if (double.Parse(Longitude) >= MaxLongitudeValue)
            {
                throw new LongitudeTooBigException(Longitude);
            }
        }

        private void ValidateLatitude()
        {
            if (string.IsNullOrWhiteSpace(Latitude))
            {
                throw new InvalidLatitudeValueException(Latitude);
            }
            
            if (double.Parse(Latitude) <= MinLatitudeValue)
            {
                throw new LatitudeTooLowException(Latitude);
            }

            if (double.Parse(Latitude) >= MaxLatitudeValue)
            {
                throw new LatitudeTooBigException(Latitude);
            }
        }
    }
}
