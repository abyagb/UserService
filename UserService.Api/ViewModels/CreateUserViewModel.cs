using System.ComponentModel.DataAnnotations;

namespace UserService.Api.ViewModels
{
    public class CreateUserViewModel
    {
        [MinLength(3)]
        [MaxLength(50)]
        public required string FirstName { get; set; }
        [MinLength(3)]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [StringLength(11)]
        public required string PhoneNumber { get; set; }
    }
}
