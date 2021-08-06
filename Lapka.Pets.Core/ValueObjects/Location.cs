namespace Lapka.Pets.Core.ValueObjects
{
    public class Location
    {
        public string Latitude { get; }
        public string Longitude { get; }
        public Location(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}