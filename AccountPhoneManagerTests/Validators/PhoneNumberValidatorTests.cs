using AccountPhoneManager.Core.Validators;
using FluentValidation.TestHelper;

namespace AccountPhoneManager.UnitTests.Validators
{
    public class PhoneNumberValidatorTests
    {
        private readonly PhoneNumberValidator _validator;

        public PhoneNumberValidatorTests()
        {
            _validator = new PhoneNumberValidator();
        }
        
        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Empty()
        {
            // Arrange
            string phoneNumber = string.Empty;

            // Act & Assert
            var result = _validator.TestValidate(phoneNumber);
            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Contains_NonNumeric_Characters()
        {
            // Arrange
            string phoneNumber = "123ABC";

            // Act & Assert
            var result = _validator.TestValidate(phoneNumber);
            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Exceeds_MaxLength()
        {
            // Arrange
            string phoneNumber = "123456789012";

            // Act & Assert
            var result = _validator.TestValidate(phoneNumber);
            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
        {
            // Arrange
            string phoneNumber = "1234567890";

            // Act & Assert
            var result = _validator.TestValidate(phoneNumber);
            result.ShouldNotHaveValidationErrorFor(x => x);
        }
    }
}

