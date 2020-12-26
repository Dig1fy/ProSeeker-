namespace ProSeeker.Services.Data.Tests
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;

    public abstract class BaseServiceTests : IDisposable
    {
        public BaseServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            this.DbContext = new ApplicationDbContext(options);
        }

        protected ApplicationDbContext DbContext { get; set; }

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
            this.DbContext.Dispose();
        }
    }
}
