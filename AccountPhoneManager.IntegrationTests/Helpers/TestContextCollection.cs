using AccountPhoneManager.IntegrationTests.TestFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPhoneManager.IntegrationTests.Helpers
{
    [CollectionDefinition("Repository Tests")]
    public class TestContextCollection : ICollectionFixture<SQLServerTestFixture>
    {
        // deliberately empty
    }
}
