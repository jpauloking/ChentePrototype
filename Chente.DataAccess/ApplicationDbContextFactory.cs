using Microsoft.EntityFrameworkCore;

namespace Chente.DataAccess;

public class ApplicationDbContextFactory
{
    public ApplicationDbContext CreateDbContext()
    {
        string connectionString = "DataSource=chente.db";
        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
        optionsBuilder.EnableSensitiveDataLogging().UseSqlite(connectionString);
        DbContextOptions<ApplicationDbContext> options = optionsBuilder.Options;
        return new ApplicationDbContext(options);
    }
}
