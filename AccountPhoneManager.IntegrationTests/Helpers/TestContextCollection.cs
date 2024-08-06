using AccountPhoneManager.IntegrationTests.TestFixtures;

namespace AccountPhoneManager.IntegrationTests.Helpers
{
    [CollectionDefinition("Repository Tests")]
    public class TestContextCollection : ICollectionFixture<SQLServerTestFixture>
    {
        // deliberately empty
    }
}
