namespace Lapka.Identity.Api.Models
{
    public class AddressModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public LocationModel GeoLocation { get; set; }
    }
}