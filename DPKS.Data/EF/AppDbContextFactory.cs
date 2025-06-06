using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DPKS.Data.EF
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-0P1NK60O;Database=qldpks;User ID=sa;Password=123;MultipleActiveResultSets=true;TrustServerCertificate=true;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
