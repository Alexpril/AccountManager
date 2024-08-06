using AccountPhoneManager.Core.Data;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.Core.Repositories;
using AccountPhoneManager.DAL.Models;
using AccountPhoneManager.IntegrationTests;
using AccountPhoneManager.IntegrationTests.TestFixtures;
using Microsoft.EntityFrameworkCore;
using Account = AccountPhoneManager.DAL.Models.Account;

namespace AccountPhoneManagerTests.Repositories
{
    public class AccountRepositoryTests(SQLServerTestFixture databaseFixture) : BaseRepositoryTests(databaseFixture)
    {
        [Fact]
        public void InsertAccount_Should_CreateAccount_Success()
        {
            // Arrange
            var accountName = "Account Name";

            using var context = CreateDataContext();
            var sut = new AccountRepository(context);

            // Act
            sut.InsertAccount(accountName);

            // Assert
            var account = context.Accounts.FirstOrDefault(a => a.Name == accountName);
            Assert.NotNull(account);
            Assert.NotEqual(Guid.Empty, account.Id);
            Assert.Equal(accountName, account.Name);
            Assert.Equal("Active", account.Status);
        }

        [Fact]
        public void GetPhoneNumbersByAccountId_Should_GetAllPhoneNumbersForAccount_Success()
        {
            // Arrange
            var phone1 = new Phone { Id = Guid.NewGuid(), Number = "789456123" };
            var phone2 = new Phone { Id = Guid.NewGuid(), Number = "123456789" };
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Status = AccountStatus.Active.ToString(),
                PhoneNumbers = new List<Phone> { phone1, phone2 }
            };

            using var context = CreateDataContext();
            context.PhoneNumbers.Add(phone1);
            context.PhoneNumbers.Add(phone2);
            context.Accounts.Add(account);
            context.SaveChanges();

            var sut = new AccountRepository(context);

            // Act
            var phones = sut.GetPhoneNumbersByAccountId(account.Id);

            // Assert
            Assert.Equal(2, phones.Count());
            Assert.Contains(phone1, phones);
            Assert.Contains(phone2, phones);
        }

        [Fact]
        public void UpdateAccount_Should_UpdateAccountStatus_Success()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active.ToString() };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.SaveChanges();
            var sut = new AccountRepository(context);

            // Act
            sut.UpdateAccount(account.Id, new UpdateAccountRequest { Status = AccountStatus.Suspended });

            // Assert
            var updatedAccount = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(updatedAccount);
            Assert.Equal(AccountStatus.Suspended.ToString(), updatedAccount.Status);
        }

        [Fact]
        public void UpdateAccount_Should_UpdateAssignedPhoneNumber_Success()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active.ToString() };
            var phone = new Phone { Id = Guid.NewGuid(), Number = "123456789", AccountId = account.Id };
            var newPhone = new Phone { Id = Guid.NewGuid(), Number = "987654312", AccountId = account.Id };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.PhoneNumbers.Add(phone);
            context.PhoneNumbers.Add(newPhone);
            context.SaveChanges();

            var sut = new AccountRepository(context);

            // Act
            sut.UpdateAccount(account.Id, new() { PhoneNumberId = newPhone.Id });

            // Assert
            var updatedAccount = context.Accounts
                .Include(a => a.PhoneNumbers)
                .FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(updatedAccount);
            Assert.Contains(updatedAccount.PhoneNumbers, p => p.Id == newPhone.Id);
        }

        [Fact]
        public void UpdateAccount_Should_UpdateAssignedPhoneNumberToSuspendedAccount_Failure()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Suspended.ToString() };
            var phone = new Phone { Id = Guid.NewGuid(), Number = "123456789", AccountId = account.Id };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.PhoneNumbers.Add(phone);
            context.SaveChanges();
            var sut = new AccountRepository(context);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => sut.UpdateAccount(account.Id, new() { PhoneNumberId = Guid.NewGuid() }));
            Assert.Equal("Cannot update phone numbers for a suspended account.", ex.Message);
        }

        [Fact]
        public void UpdateAccount_Should_UpdateAssignedPhoneNumberWhenItsAlreadyAssigned_Failure()
        {
            // Arrange
            var phoneNumber = Guid.NewGuid();
            var account1 = new Account { Id = Guid.NewGuid(), Name = "Account1", Status = AccountStatus.Active.ToString(), PhoneNumbers = new List<Phone> { new Phone { Id = phoneNumber, Number = "123456789" } } };
            var account2 = new Account { Id = Guid.NewGuid(), Name = "Account2", Status = AccountStatus.Active.ToString() };

            using var context = CreateDataContext();
            context.Accounts.Add(account1);
            context.Accounts.Add(account2);
            context.SaveChanges();

            var sut = new AccountRepository(context);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => sut.UpdateAccount(account2.Id, new() { PhoneNumberId = phoneNumber }));
            Assert.Equal("Phone number is already assigned to another account.", ex.Message);
        }
    }
}
