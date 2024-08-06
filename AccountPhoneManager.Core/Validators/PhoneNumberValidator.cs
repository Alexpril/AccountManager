using FluentValidation;

namespace AccountPhoneManager.Core.Validators
{

    public class PhoneNumberValidator : AbstractValidator<string>
    {
        public PhoneNumberValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("Phone number cannot be empty.")
                .Matches("^[0-9]+$").WithMessage("Phone number must contain only numeric characters.")
                .MaximumLength(11).WithMessage("Phone number must not exceed 11 digits.");
        }
    }
}
