using FluentValidation;

namespace AccountPhoneManager.Core.Validators
{

    public class PhoneNumberValidator : AbstractValidator<IEnumerable<string>>
    {
        public PhoneNumberValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("Phone number cannot be empty.");
        }
    }
}
