using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chente.DataAccess;

internal class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        string connectionString = "DataSource=chente.db";
        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlite(connectionString);
        DbContextOptions<ApplicationDbContext> options = optionsBuilder.Options;
        return new ApplicationDbContext(options);
    }
}
