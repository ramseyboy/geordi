using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Geordi.Fixtures;

public interface IDbConnectionConfiguration
{
    DbConnection Connection { get; }

    void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder,
        QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.NoTracking);
}
