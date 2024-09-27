using Geordi.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Geordi.Test.Database;

public class TestContext(IDbConnectionConfiguration configuration, ILoggerFactory loggerFactory): DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        configuration.OnConfiguring(optionsBuilder);
    }
}
