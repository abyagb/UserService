using FluentValidation;
using UserService.Api.ViewModels;

namespace UserService.Api.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserValidator()
        {
            ApplyNameRules(() => RuleFor(x => x.FirstName));

            ApplyNameRules(() => RuleFor(x => x.LastName));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{11}$") // Only allows exactly 11 digits
                .WithMessage("Phone number must be exactly 11 digits.");
        }

        private static void ApplyNameRules(Func<IRuleBuilderInitial<CreateUserViewModel, string>> ruleFor)
        {
            ruleFor()
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MinimumLength(3).WithMessage("Minimum length is 3 characters.")
                .MaximumLength(50).WithMessage("Maximum length is 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Name must contain only letters without spaces or special characters.");
        }

    }
}

