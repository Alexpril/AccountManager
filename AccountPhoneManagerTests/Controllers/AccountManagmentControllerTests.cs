using AccountPhoneManager.API.Controllers;
using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

public class AccountControllerTests
{
    private readonly Mock<IAccountManagmentService> _mockService;
    private readonly AccountManagmentController _controller;

    public AccountControllerTests()
    {
        var mocker = new AutoMocker();
        _mockService = mocker.GetMock<IAccountManagmentService>();
        _controller = new AccountManagmentController(_mockService.Object);
    }

    [Theory]
    [InlineData("Test Account")]
    public void CreateAccount_Success_ReturnsOk(string name)
    {
        // Act
        var result = _controller.CreateAccount(name) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Account created successfully.", result.Value);
    }

    [Theory]
    [InlineData("")]
    public void CreateAccount_InvalidName_ReturnsBadRequest(string name)
    {
        // Arrange
        _mockService.Setup(x => x.CreateAccount(name))
            .Throws(new ArgumentException("Account name cannot be null or empty.", nameof(name)));

        // Act
        var result = _controller.CreateAccount(name) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Account name cannot be null or empty.", result.Value);
    }

    [Fact]
    public void GetPhoneNumbersByAccountId_Success_ReturnsOk()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var phone = new Phone { Id = Guid.NewGuid(), Number = "789456123", AccountId = accountId };
        var phone2 = new Phone { Id = Guid.NewGuid(), Number = "789456123", AccountId = accountId };
        _mockService.Setup(x => x.GetPhoneNumbersByAccountId(accountId))
            .Returns([phone, phone2]);

        // Act
        var result = _controller.GetPhoneNumbersByAccountId(accountId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void GetPhoneNumbersByAccountId_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _mockService.Setup(x => x.GetPhoneNumbersByAccountId(accountId))
            .Throws(new ArgumentException("Account ID cannot be empty.", nameof(accountId)));

        // Act
        var result = _controller.GetPhoneNumbersByAccountId(accountId) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Account ID cannot be empty.", result.Value);
    }

    [Theory]
    [InlineData(AccountStatus.Active)]
    [InlineData(AccountStatus.Suspended)]
    public void UpdateAccountStatus_Success_ReturnsOk(AccountStatus status)
    {
        // Arrange
        var accountId = Guid.NewGuid();

        // Act
        var result = _controller.UpdateAccount(accountId, status) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Account updated successfully.", result.Value);
    }

    [Fact]
    public void UpdateAccountNumber_Success_ReturnsOk()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var phoneId = Guid.NewGuid();

        // Act
        var result = _controller.UpdateAccount(accountId, phoneId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Account updated successfully.", result.Value);
    }

    [Fact]
    public void UpdateAccountNumber_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var phoneId = Guid.NewGuid();
        _mockService.Setup(x => x.UpdateAccountNumber(accountId, phoneId))
            .Throws(new ArgumentException("Account ID cannot be empty.", nameof(accountId)));

        // Act
        var result = _controller.UpdateAccount(accountId, phoneId) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Account ID cannot be empty.", result.Value);
    }
}