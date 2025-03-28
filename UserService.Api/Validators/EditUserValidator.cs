// Homework to do write validator for editing a user
// The validation rules here are the same for creating a user except values don't need to be null
using FluentValidation;
using UserService.ViewModels;

namespace UserService.Validators
{
    public class EditUserValidator : AbstractValidator<EditUserViewModel>
    {
        public EditUserValidator()
        {
            ApplyNameRules(RuleFor(x=>x.FirstName));

            ApplyNameRules(RuleFor(x => x.LastName));


            RuleFor(x => x.Email)
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Invalid email format.")
                .When(x=> !string.IsNullOrEmpty(x.Email));
            


            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{11}$") // Only allows exactly 11 digits
                .WithMessage("Phone number must be exactly 11 digits.")
                .When(x=> !string.IsNullOrEmpty(x.PhoneNumber));
        }

        private  void ApplyNameRules(IRuleBuilderInitial<EditUserViewModel, string?> ruleFor)
        {
            ruleFor
             .Must(x => string.IsNullOrEmpty(x) || x.Length >= 3)
             .WithMessage("Minimum length is 3 characters.")
             .Must(x => string.IsNullOrEmpty(x) || x.Length <= 50)
             .WithMessage("Maximum length is 50 characters.")
             .Matches("^[a-zA-Z]+$")
             .WithMessage("Name must contain only letters without spaces or special characters.");
        }
    }
}
