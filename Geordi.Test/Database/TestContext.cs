using Geordi.Fixtures;
using Geordi.Test.Database.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Geordi.Test.Database;

public class TestContext(IDbConnectionConfiguration configuration, ILoggerFactory loggerFactory): DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        configuration.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContractConfiguration());
        modelBuilder.ApplyConfiguration(new ContractTypeConfiguration());
    }
}
