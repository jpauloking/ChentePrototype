using Chente.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Chente.DataAccess.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContextFactory contextFactory;

    public UserRepository(ApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task CreateAsync(User user)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        User? borrowerFromDatabase = await context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (borrowerFromDatabase is not null)
        {
            context.Users.Remove(borrowerFromDatabase);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> GetAsync()
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<User> borrowers = await context.Users.AsNoTracking().ToListAsync();
        return borrowers;
    }

    public async Task<User?> GetAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
        return user;
    }

    public async Task<User?> GetAsync(string emailAddress)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.EmailAddress == emailAddress);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}
