using Essays.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Tests
{
    public class TestDatabaseGenerator
    {
        public async Task<DataContext> GetDatabase()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataContext = new DataContext(options);
            var seeder = new Seeder(dataContext);
            await seeder.Seed();

            return dataContext;
        }
    }
}
