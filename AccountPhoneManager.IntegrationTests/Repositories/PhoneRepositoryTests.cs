using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.Core.Repositories;
using AccountPhoneManager.DAL.Models;
using AccountPhoneManager.IntegrationTests;
using AccountPhoneManager.IntegrationTests.TestFixtures;
using Account = AccountPhoneManager.DAL.Models.Account;

namespace AccountPhoneManagerTests.Repositories
{
    public class PhoneRepositoryTests(SQLServerTestFixture databaseFxture) : BaseRepositoryTests(databaseFxture)
    {
        [Fact]
        public void Should_CreatePhoneNumber_Success()
        {
            // Arrange
            var phoneNumber = "8978789845";

            using var context = CreateDataContext();
            var sut = new PhoneRepository(context);

            // Act
            sut.CreatePhoneNumber(phoneNumber);

            // Assert
            var actualPhoneNumber = context.PhoneNumbers.FirstOrDefault(a => a.Number == phoneNumber);
            Assert.NotNull(actualPhoneNumber);
            Assert.NotEqual(Guid.Empty, actualPhoneNumber.Id);
            Assert.Equal(phoneNumber, actualPhoneNumber.Number);
            Assert.False(actualPhoneNumber.IsDeleted);
        }

        [Fact]
        public void Should_GetAllPhoneNumbersForAccount_Success()
        {
            // Arrange
            var phone = new Phone() { Id = Guid.NewGuid(), Number = "789456123" };
            var phone2 = new Phone() { Id = Guid.NewGuid(), Number = "123456789" };
            var account = new Account() { Id = Guid.NewGuid(), Name = "Name", Status = (AccountStatus.Active).ToString(), PhoneNumberIds = [phone.Id, phone2.Id] };

            using var context = CreateDataContext();
            context.PhoneNumbers.Add(phone);
            context.PhoneNumbers.Add(phone2);
            context.Accounts.Add(account);
            context.SaveChanges();
            var sut = new PhoneRepository(context);

            // Act
            var phones = sut.ReadPhoneNumbersByAccountId(account.Id);

            // Assert
            Assert.Equal(2, phones.Count());
            Assert.Contains(phone, phones);
            Assert.Contains(phone2, phones);
        }

        [Fact]
        public void Should_DeletePhoneNumber_Success()
        {
            // Arrange
            var phone = new Phone() { Id = Guid.NewGuid(), Number = "789456123" };

            using var context = CreateDataContext();
            context.PhoneNumbers.Add(phone);
            context.SaveChanges();
            var sut = new PhoneRepository(context);

            // Act
            sut.DeletePhoneNumber(phone.Id);

            // Assert
            var deletedPhoneNumber = context.PhoneNumbers.FirstOrDefault(a => a.Number == phone.Number);
            Assert.NotNull(deletedPhoneNumber);
            Assert.True(deletedPhoneNumber.IsDeleted);
        }

        [Fact]
        public void Should_DeletePhoneNumber_And_UnassignFromAccount_Success()
        {
            // Arrange
            var phone = new Phone() { Id = Guid.NewGuid(), Number = "789456123" };
            var account = new Account() { Id = Guid.NewGuid(), Name = "Name", Status = (AccountStatus.Active).ToString(), PhoneNumberIds = [phone.Id] };

            using var context = CreateDataContext();
            context.PhoneNumbers.Add(phone);
            context.Accounts.Add(account);
            context.SaveChanges();
            var sut = new PhoneRepository(context);

            // Act
            sut.DeletePhoneNumber(phone.Id);

            // Assert
            var deletedPhoneNumber = context.PhoneNumbers.FirstOrDefault(a => a.Number == phone.Number);
            Assert.NotNull(deletedPhoneNumber);
            Assert.True(deletedPhoneNumber.IsDeleted);

            var accountAfterPhoneDeletion = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(accountAfterPhoneDeletion);
            Assert.DoesNotContain(deletedPhoneNumber.Id, accountAfterPhoneDeletion.PhoneNumberIds); // Check if unassigned
        }
    }
}
