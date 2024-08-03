using AccountPhoneManager.IntegrationTests.TestFixtures;

namespace AccountPhoneManager.IntegrationTests
{
    public class AccountRepositoryTests(SQLServerTestFixture databaseFxture) : BaseRepositoryTests(databaseFxture)
    {
        [Fact]
        public void Shoud_CreateAccount_Success()
        {
            // Arrange
            using var context = CreateDataContext();
            var accountName = "Account Name";
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
    }
}
