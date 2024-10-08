﻿using AccountPhoneManager.DAL.Contexts;
using AccountPhoneManager.IntegrationTests.TestFixtures;
using AutoFixture;

namespace AccountPhoneManager.IntegrationTests
{
    [Collection("Repository Tests")]
    public abstract class BaseRepositoryTests
    {
        private readonly SQLServerTestFixture _databaseFixture;

        public Fixture Fixture => _databaseFixture.Fixture;

        protected BaseRepositoryTests(SQLServerTestFixture databaseFixture) 
        {
            _databaseFixture = databaseFixture;
            _databaseFixture.Reset();
        }

        protected AccountManagerDbContexts CreateDataContext() => _databaseFixture.CreateAccountManagerContext();
    }
}
