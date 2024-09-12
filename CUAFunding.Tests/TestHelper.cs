using CUAFunding.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CUAFunding.Tests
{
    public class TestHelper
    {
        private readonly ApplicationDbContext _context;

        public TestHelper(string dbName = "InMemoryDb")
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: dbName);

            var dbContextOptions = builder.Options;
            _context = new ApplicationDbContext(dbContextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public ApplicationDbContext GetInMemoryRepo()
        {
            return _context;
        }

        public IFormFile GetFormFileFromFile(string ImagePath)
        {

            IFormFile formFile;
            using (FileStream streamReader = File.OpenRead(ImagePath))
            {
                var mstream = new MemoryStream();
                streamReader.CopyTo(mstream);
                formFile = new FormFile(mstream, 0, mstream.Length, null, "TestFileName_" + Path.GetFileName(ImagePath));
            }
            return formFile;
        }
    }

}
