using AccountPhoneManager.DAL.Contexts;
using AutoFixture;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.MsSql;

namespace AccountPhoneManager.IntegrationTests.TestFixtures
{
    public class SQLServerTestFixture : IAsyncDisposable
    {
        public Fixture Fixture;

        private readonly MsSqlContainer _container;
        private readonly DbContextOptionsBuilder<AccountManagerDbContexts> _contextBuilder;
        private Respawner? _respawner;

        public string ConnectionString => $"{_container.GetConnectionString()}";

        public SQLServerTestFixture()
        {
            Fixture = SetupAutoFixture();
            _container = BuildContainer();
            _contextBuilder = SetupContext();
            MigrateDatabase();
        }

        public AccountManagerDbContexts CreateAccountManagerContext()
        {
            return new AccountManagerDbContexts(_contextBuilder!.Options);
        }

        public void Reset()
        {
            Console.WriteLine("Resetting Database");

            using var conn = new SqlConnection(ConnectionString);
            conn.OpenAsync().Wait();

            _respawner = Respawner.CreateAsync(conn, new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer
            }).Result;

            _respawner.ResetAsync(conn).Wait();
        }

        public ValueTask DisposeAsync()
        {
            Console.WriteLine("Disposing of Container Services");
            GC.SuppressFinalize(this);
            return _container!.DisposeAsync();
        }

        private static Fixture SetupAutoFixture()
        {
            // Setup Autofixture to help with automatic test data creation
            var fixture = new Fixture();

            // Configure Autofixture to use OmitOnRecursionBehavior
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }

        private static MsSqlContainer BuildContainer()
        {
            Console.WriteLine("Building Sql server Test Container");

            try
            {
                var container = new MsSqlBuilder().Build();
                Console.WriteLine("Starting Container Services");
                container.StartAsync().Wait();

                return container;
            }
            catch (Exception)
            {
                throw new Exception("Make sure Docker is running - it is required to spin-up container");
            }
        }

        private DbContextOptionsBuilder<AccountManagerDbContexts> SetupContext()
        {
            return new DbContextOptionsBuilder<AccountManagerDbContexts>().UseSqlServer(ConnectionString);
        }

        private void MigrateDatabase()
        {
            Console.WriteLine("Migration Database");
            using var migrationContext = new AccountManagerDbContexts(_contextBuilder!.Options);
            migrationContext.Database.Migrate();

        }
    }
}
