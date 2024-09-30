using Geordi.Seeder;
using Geordi.Test.Fixtures;
using Xunit.Abstractions;

namespace Geordi.Test.Seeder;

public class EntitySeederTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task TestSeeder()
    {
        var db = new DatabaseTestFixture(testOutputHelper);
        var seeder = new EntitySeeder(db.Context);
        await seeder.Seed();
    }
}
