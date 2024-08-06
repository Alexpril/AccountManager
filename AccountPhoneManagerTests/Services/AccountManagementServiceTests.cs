using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.Core.Services;
using AccountPhoneManager.DAL.Models;
using Moq.AutoMock;
using Moq;
using AccountPhoneManager.Core.Data;

namespace AccountPhoneManager.UnitTests.Services
{
    public class AccountManagmentServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly AccountManagmentService _service;

        public AccountManagmentServiceTests()
        {
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<AccountManagmentService>();
        }

        [Fact]
        public void CreateAccount_Should_ThrowException_When_NameIsNullOrEmpty()
        {
            // Arrange
            string accountName = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.CreateAccount(accountName));
            Assert.Equal("Account name cannot be null or empty. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void CreateAccount_Should_CallRepository_When_NameIsValid()
        {
            // Arrange
            var accountName = "Valid Account Name";

            // Act
            _service.CreateAccount(accountName);

            // Assert
            _mocker.GetMock<IAccountRepository>().Verify(r => r.InsertAccount(accountName), Times.Once);
        }

        [Fact]
        public void ReadPhoneNumbersByAccountId_Should_ThrowException_When_AccountIdIsEmpty()
        {
            // Arrange
            var accountId = Guid.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.GetPhoneNumbersByAccountId(accountId));
            Assert.Equal("Account ID cannot be empty. (Parameter 'accountId')", exception.Message);
        }

        [Fact]
        public void ReadPhoneNumbersByAccountId_Should_CallRepository_When_AccountIdIsValid()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var phoneNumbers = new List<Phone> { new Phone { Id = Guid.NewGuid(), Number = "123456789" } };
            _mocker.GetMock<IAccountRepository>().Setup(r => r.GetPhoneNumbersByAccountId(accountId)).Returns(phoneNumbers);

            // Act
            var result = _service.GetPhoneNumbersByAccountId(accountId);

            // Assert
            Assert.Equal(phoneNumbers, result);
            _mocker.GetMock<IAccountRepository>().Verify(r => r.GetPhoneNumbersByAccountId(accountId), Times.Once);
        }

        [Fact]
        public void UpdateAccountStatus_Should_ThrowException_When_AccountIdIsEmpty()
        {
            // Arrange
            var accountId = Guid.Empty;
            var status = AccountStatus.Active;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateAccountStatus(accountId, status));
            Assert.Equal("Account ID cannot be empty. (Parameter 'accountId')", exception.Message);
        }

        [Fact]
        public void UpdateAccountStatus_Should_CallRepository_When_AccountIdIsValid()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var status = AccountStatus.Active;

            // Act
            _service.UpdateAccountStatus(accountId, status);

            // Assert
            _mocker.GetMock<IAccountRepository>().Verify(r => r.UpdateAccount(accountId, It.Is<UpdateAccountRequest>(u => u.Status == status)), Times.Once);
        }

        [Fact]
        public void UpdateAccountNumber_Should_ThrowException_When_AccountIdIsEmpty()
        {
            // Arrange
            var accountId = Guid.Empty;
            var phoneId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateAccountNumber(accountId, phoneId));
            Assert.Equal("Account ID cannot be empty. (Parameter 'accountId')", exception.Message);
        }

        [Fact]
        public void UpdateAccountNumber_Should_CallRepository_When_AccountIdIsValid()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var phoneId = Guid.NewGuid();

            // Act
            _service.UpdateAccountNumber(accountId, phoneId);

            // Assert
            _mocker.GetMock<IAccountRepository>().Verify(r => r.UpdateAccount(accountId, It.Is<UpdateAccountRequest>(u => u.PhoneNumberId == phoneId)), Times.Once);
        }
    }
}