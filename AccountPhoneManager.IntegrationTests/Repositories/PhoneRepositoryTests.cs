using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.Core.Repositories;
using AccountPhoneManager.DAL.Models;
using AccountPhoneManager.IntegrationTests;
using AccountPhoneManager.IntegrationTests.TestFixtures;
using Microsoft.EntityFrameworkCore;

namespace AccountPhoneManagerTests.Repositories
{
    public class PhoneRepositoryTests(SQLServerTestFixture databaseFixture) : BaseRepositoryTests(databaseFixture)
    {
        [Fact]
        public void InsertPhoneNumber_Should_CreatePhoneNumber_Success()
        {
            // Arrange
            var phoneNumber = "8978789845";

            using var context = CreateDataContext();
            var sut = new PhoneRepository(context);

            // Act
            sut.InsertPhoneNumber(phoneNumber);

            // Assert
            var actualPhoneNumber = context.PhoneNumbers.FirstOrDefault(a => a.Number == phoneNumber);
            Assert.NotNull(actualPhoneNumber);
            Assert.NotEqual(Guid.Empty, actualPhoneNumber.Id);
            Assert.Equal(phoneNumber, actualPhoneNumber.Number);
        }

        [Fact]
        public void DeletePhoneNumber_Should_DeletePhoneNumber_Success()
        {
            // Arrange
            var phone = new Phone { Id = Guid.NewGuid(), Number = "789456123" };

            using var context = CreateDataContext();
            context.PhoneNumbers.Add(phone);
            context.SaveChanges();

            var sut = new PhoneRepository(context);

            // Act
            sut.DeletePhoneNumber(phone.Id);

            // Assert
            var deletedPhoneNumber = context.PhoneNumbers.FirstOrDefault(a => a.Id == phone.Id);
            Assert.NotNull(deletedPhoneNumber);
        }

        [Fact]
        public void DeletePhoneNumber_Should_DeletePhoneNumber_And_UnassignFromAccount_Success()
        {
            // Arrange
            var phone = new Phone { Id = Guid.NewGuid(), Number = "789456123" };
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Status = AccountStatus.Active.ToString(),
                PhoneNumbers = new List<Phone> { phone }
            };

            using var context = CreateDataContext();
            context.PhoneNumbers.Add(phone);
            context.Accounts.Add(account);
            context.SaveChanges();

            var sut = new PhoneRepository(context);

            // Act
            sut.DeletePhoneNumber(phone.Id);

            // Assert
            var deletedPhoneNumber = context.PhoneNumbers.FirstOrDefault(a => a.Id == phone.Id);
            Assert.NotNull(deletedPhoneNumber);

            var accountAfterPhoneDeletion = context.Accounts
                .Include(a => a.PhoneNumbers)
                .FirstOrDefault(a => a.Id == account.Id);

            Assert.NotNull(accountAfterPhoneDeletion);
            Assert.DoesNotContain(deletedPhoneNumber, accountAfterPhoneDeletion.PhoneNumbers);
        }
    }
}
