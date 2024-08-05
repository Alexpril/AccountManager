using AccountPhoneManager.DAL.Models;
using AccountPhoneManager.IntegrationTests.TestFixtures;

namespace AccountPhoneManager.IntegrationTests
{
    public class AccountRepositoryTests(SQLServerTestFixture databaseFxture) : BaseRepositoryTests(databaseFxture)
    {
        [Fact]
        public void Shoud_CreateAccount_Success()
        {
            // Arrange
            var accountName = "Account Name";

            using var context = CreateDataContext();
            var sut = new AccountRepository(context);

            // Act
            sut.CreateAccount(accountName);

            // Assert
            var account = context.Accounts.FirstOrDefault(a => a.Name == accountName);
            Assert.NotNull(account);
            Assert.NotEqual(Guid.Empty, account.Id);
            Assert.Equal(accountName, account.Name);
            Assert.Equal("Active", account.Status);
        }

        [Fact]
        public void Shoud_UpdateAccountStatus_Success()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active, PhoneNumberId = Guid.NewGuid() };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            var sut = new AccountRepository(context);

            // Act
            sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { Status = AccountStatus.Suspended } );

            // Assert
            var updatedAccount = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(updatedAccount);
            Assert.Equal(AccountStatus.Suspended, updatedAccount.Status);
        }

        [Fact]
        public void Shoud_UpdateAssignedPhoneNumber_Success()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active, PhoneNumberId = Guid.NewGuid() };
            var newPhoneNumber = Guid.NewGuid();

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            var sut = new AccountRepository(context);

            // Act
            sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { PhoneNumberId = newPhoneNumber });

            // Assert
            var updatedAccount = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(updatedAccount);
            Assert.Equal(newPhoneNumber, updatedAccount.PhoneNumberId);
        }

        [Fact]
        public void Shoud_UpdateAssignedPhoneNumberToSuspendedAccount_Failure()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Suspended, PhoneNumberId = Guid.NewGuid() };
            var newPhoneNumber = Guid.NewGuid();

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            var sut = new AccountRepository(context);

            // Act
            Assert.Throws<Exception>(() => sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { PhoneNumberId = newPhoneNumber }));
        }

        [Fact]
        public void Shoud_UpdateAssignedPhoneNumberWhenItsAlreadyAssigned_Failure()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active, PhoneNumberId = Guid.NewGuid() };

            var newPhoneNumber = Guid.NewGuid();
            var account2 = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active, PhoneNumberId = newPhoneNumber };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.Accounts.Add(account2);
            var sut = new AccountRepository(context);

            // Act
            Assert.Throws<Exception>(() => sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { PhoneNumberId = newPhoneNumber }));
        }
    }
}
