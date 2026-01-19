using MySqlAnalyzer.Repositories;

using Xunit;

namespace MySqlAnalyzer.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class DatabaseIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DatabaseIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ConnectToTestDatabase_Success()
        {
            // Arrange
            var repository = new DatabaseRepository(_fixture.ConnectionString);

            // Act
            var databases = repository.GetDatabaseNames();

            // Assert
            Assert.NotNull(databases);
            Assert.Contains(_fixture.TestDatabaseName, databases);
        }
    }

    public class DatabaseFixture : IDisposable
    {
        public string ConnectionString { get; }
        public string TestDatabaseName { get; }

        public DatabaseFixture()
        {
            ConnectionString = "Server=localhost;Uid=root;Pwd=test;";
            TestDatabaseName = "TestDatabase_Analyzer";

            // Setup test database
            SetupTestDatabase();
        }

        private void SetupTestDatabase()
        {
            // Code om test database aan te maken met test data
        }

        public void Dispose()
        {
            // Cleanup test database
        }
    }
}