using UserService.Domain;

namespace UserService.Application.DTOs
{
    public class UserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required Address Address { get; set; }
    }
}
