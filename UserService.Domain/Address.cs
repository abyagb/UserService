namespace UserService.Domain
{
    public class Address
    {
        public  int DoorNumber { get; set; }
        public required string StreetName {  get; set; }
        public required string City { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }
}
