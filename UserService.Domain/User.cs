namespace UserService.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required Address Address { get; set; }

    }
}

