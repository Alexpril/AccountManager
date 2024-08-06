using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace AccountPhoneManagerTests.Controllers
{
    public class PhoneManagmentControllerTests
    {
        private readonly Mock<IPhoneManagmentService> _mockService;
        private readonly PhoneManagmentController _controller;

        public PhoneManagmentControllerTests()
        {
            var mocker = new AutoMocker();
            _mockService = mocker.GetMock<IPhoneManagmentService>();
            _controller = new PhoneManagmentController(_mockService.Object);
        }

        [Fact]
        public void CreatePhoneNumber_ValidPhoneNumber_ReturnsOk()
        {
            // Arrange
            var phoneNumber = "1234567890";

            // Act
            var result = _controller.CreatePhoneNumber(phoneNumber) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Phone number created successfully.", result.Value);
        }

        [Fact]
        public void CreatePhoneNumber_EmptyPhoneNumber_ReturnsBadRequest()
        {
            // Arrange
            var phoneNumber = string.Empty;
            _mockService
                .Setup(service => service.CreatePhoneNumber(phoneNumber))
                .Throws(new ArgumentException("Phone number cannot be null or empty."));

            // Act
            var result = _controller.CreatePhoneNumber(phoneNumber) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Phone number cannot be null or empty.", result.Value);
        }

        [Fact]
        public void DeletePhoneNumber_ValidPhoneId_ReturnsOk()
        {
            // Arrange
            var phoneId = Guid.NewGuid();

            // Act
            var result = _controller.DeletePhoneNumber(phoneId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Phone number deleted successfully.", result.Value);
        }

        [Fact]
        public void DeletePhoneNumber_InvalidPhoneId_ReturnsBadRequest()
        {
            // Arrange
            var phoneId = Guid.Empty;
            _mockService
                .Setup(service => service.DeletePhoneNumber(phoneId))
                .Throws(new ArgumentException("Phone ID cannot be empty."));

            // Act
            var result = _controller.DeletePhoneNumber(phoneId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Phone ID cannot be empty.", result.Value);
        }

        [Fact]
        public void DeletePhoneNumber_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var phoneId = Guid.NewGuid();
            _mockService
                .Setup(service => service.DeletePhoneNumber(phoneId))
                .Throws(new Exception("Internal server error."));

            // Act
            var result = _controller.DeletePhoneNumber(phoneId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Internal server error.", result.Value);
        }
    }
}