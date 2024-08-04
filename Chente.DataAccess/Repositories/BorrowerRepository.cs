using Chente.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Chente.DataAccess.Repositories;

public class BorrowerRepository
{
    private readonly ApplicationDbContextFactory contextFactory;

    public BorrowerRepository(ApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task CreateAsync(Borrower borrower)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Borrowers.Add(borrower);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        Borrower? borrowerFromDatabase = await context.Borrowers.FirstOrDefaultAsync(b => b.Id == id);
        if (borrowerFromDatabase is not null)
        {
            context.Borrowers.Remove(borrowerFromDatabase);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Borrower>> GetAsync()
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<Borrower> borrowers = await context.Borrowers.AsNoTracking().ToListAsync();
        return borrowers;
    }

    public async Task<Borrower?> GetAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        Borrower? borrower = await context.Borrowers.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        return borrower;
    }

    public async Task UpdateAsync(Borrower borrower)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Borrowers.Update(borrower);
        await context.SaveChangesAsync();
    }
}
