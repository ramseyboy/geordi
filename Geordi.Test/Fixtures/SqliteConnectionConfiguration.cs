using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Geordi.Fixtures
{
    public sealed class SqliteConnectionConfiguration: IDbConnectionConfiguration
    {
        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder, QueryTrackingBehavior trackingBehavior)
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.CreateFunction("getdate", () => DateTime.Now);
            connection.CreateFunction("newid", Guid.NewGuid);

            optionsBuilder
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseQueryTrackingBehavior(trackingBehavior);

            Connection = connection;
        }

        public DbConnection Connection { get; private set; } = null!;
    }
}
