using FluentValidation;
using UserService.Api.ViewModels;
using UserService.Domain;

namespace UserService.Api.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserValidator()
        {
            ApplyNameRules(() => RuleFor(x => x.FirstName));

            ApplyNameRules(() => RuleFor(x => x.LastName));

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number cannot be empty.")
                .Length(11).WithMessage("The length of phone number should be 11 characters.");
        }

        private void ApplyNameRules(Func<IRuleBuilderInitial<CreateUserViewModel, string>> ruleFor)
        {
            ruleFor()
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MinimumLength(3).WithMessage("Minimum length is 3 characters.")
                .MaximumLength(50).WithMessage("Maximum length is 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Name must contain only letters without spaces or special characters.");
        }

    }
}
