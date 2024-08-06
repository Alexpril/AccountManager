using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Services;
using AccountPhoneManager.DAL.Models;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;

namespace AccountPhoneManagerTests.Services
{
    public class PhoneManagmentServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly PhoneManagmentService _service;

        public PhoneManagmentServiceTests()
        {
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<PhoneManagmentService>();
        }

        [Fact]
        public void CreatePhoneNumber_Should_Call_InsertPhoneNumber_With_Valid_Number()
        {
            // Arrange
            var phoneNumber = "1234567890";
            _mocker.GetMock<IValidator<string>>().Setup(x => x.Validate(It.IsAny<string>())).Returns(new ValidationResult());

            // Act
            _service.CreatePhoneNumber(phoneNumber);

            // Assert
            _mocker.GetMock<IPhoneRepository>()
                   .Verify(r => r.InsertPhoneNumber(phoneNumber), Times.Once);
        }

        [Fact]
        public void DeletePhoneNumber_Should_Call_DeletePhoneNumber_With_Valid_Id()
        {
            // Arrange
            var phoneId = Guid.NewGuid();

            // Act
            _service.DeletePhoneNumber(phoneId);

            // Assert
            _mocker.GetMock<IPhoneRepository>()
                   .Verify(r => r.DeletePhoneNumber(phoneId), Times.Once);
        }

        [Fact]
        public void DeletePhoneNumber_Should_Throw_ArgumentException_When_PhoneId_Is_Empty()
        {
            // Arrange
            var phoneId = Guid.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.DeletePhoneNumber(phoneId));
            Assert.Equal("Phone ID cannot be empty. (Parameter 'phoneId')", exception.Message);
        }
    }
}
