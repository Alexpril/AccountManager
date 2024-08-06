using AccountPhoneManager.Core.Data;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.Core.Repositories;
using AccountPhoneManager.DAL.Models;
using AccountPhoneManager.IntegrationTests;
using AccountPhoneManager.IntegrationTests.TestFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account = AccountPhoneManager.DAL.Models.Account;

namespace AccountPhoneManagerTests.Repositories
{
    public class AccountRepositoryTests(SQLServerTestFixture databaseFxture) : BaseRepositoryTests(databaseFxture)
    {
        [Fact]
        public void Should_CreateAccount_Success()
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
        public void Should_UpdateAccountStatus_Success()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = (AccountStatus.Active).ToString(), PhoneNumberIds = [Guid.NewGuid()] };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.SaveChanges();
            var sut = new AccountRepository(context);

            // Act
            sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { Status = AccountStatus.Suspended });

            // Assert
            var updatedAccount = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(updatedAccount);
            Assert.Equal("Suspended", updatedAccount.Status);
        }

        [Fact]
        public void Should_UpdateAssignedPhoneNumber_Success()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active.ToString() };
            var newPhoneNumber = Guid.NewGuid();

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.SaveChanges();

            var sut = new AccountRepository(context);

            // Act
            sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { PhoneNumberId = newPhoneNumber });

            // Assert
            var updatedAccount = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(updatedAccount);
            Assert.Equal(newPhoneNumber, updatedAccount.PhoneNumberIds.First());
        }

        [Fact]
        public void Should_UpdateAssignedPhoneNumberToSuspendedAccount_Failure()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Suspended.ToString(), PhoneNumberIds = [Guid.NewGuid()] };
            var newPhoneNumber = Guid.NewGuid();

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.SaveChanges();
            var sut = new AccountRepository(context);

            // Act
            var ex = Assert.Throws<Exception>(() => sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { PhoneNumberId = newPhoneNumber }));
            Assert.Equal("Cannot update phone numbers for a suspended account.", ex.Message);
        }

        [Fact]
        public void Should_UpdateAssignedPhoneNumberWhenItsAlreadyAssigned_Failure()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active.ToString(), PhoneNumberIds = [Guid.NewGuid()] };

            var newPhoneNumber = Guid.NewGuid();
            var account2 = new Account { Id = Guid.NewGuid(), Name = "Name", Status = AccountStatus.Active.ToString(), PhoneNumberIds = [newPhoneNumber] };

            using var context = CreateDataContext();
            context.Accounts.Add(account);
            context.Accounts.Add(account2);
            context.SaveChanges();
            var sut = new AccountRepository(context);

            // Act
            var ex = Assert.Throws<Exception>(() => sut.UpdateAccountStatus(account.Id, new UpdateAccountRequest() { PhoneNumberId = newPhoneNumber }));
            Assert.Equal("Phone number is already assigned to another account.", ex.Message);
        }
    }
}
