namespace Lapka.Pets.Api.Models.Request
{
    public class UpdateAddressRequest
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public UpdateLocationRequest GeoLocation { get; set; }
    }
}