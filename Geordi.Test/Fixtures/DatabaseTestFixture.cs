using FakeItEasy;
using Geordi.Fixtures;
using Geordi.Test.Database;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Geordi.Test.Fixtures
{
    public sealed class DatabaseTestFixture : IDisposable
    {
        public DatabaseTestFixture(ITestOutputHelper testOutputHelper)
        {
            var connectionConfiguration = new SqliteConnectionConfiguration();
            var loggerFactory = new XUnitLoggerFactory(testOutputHelper);

            Context = new TestContext(connectionConfiguration, loggerFactory);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();

            ContextFactory = A.Fake<IDbContextFactory<DbContext>>();
            A.CallTo(() => ContextFactory.CreateDbContext())
                .Returns(Context);
            A.CallTo(() => ContextFactory.CreateDbContextAsync(default))
                .Returns(Task.FromResult(Context));
        }

        public DbContext Context { get; }

        public IDbContextFactory<DbContext> ContextFactory { get; }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
