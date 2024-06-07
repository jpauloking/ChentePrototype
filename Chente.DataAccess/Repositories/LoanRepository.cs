using Chente.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Chente.DataAccess.Repositories;

public class LoanRepository
{
    private readonly ApplicationDbContextFactory contextFactory;

    public LoanRepository(ApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task CreateAsync(Loan loan)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Loans.Add(loan);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        Loan? loanFromDatabase = await context.Loans.FirstOrDefaultAsync(b => b.Id == id);
        if (loanFromDatabase is not null)
        {
            context.Loans.Remove(loanFromDatabase);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Loan>> GetAsync()
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<Loan> loans = await context.Loans.ToListAsync();
        return loans;
    }

    public async Task<Loan?> GetAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        Loan? loans = await context.Loans.Include(l => l.Installments).FirstOrDefaultAsync(l => l.Id == id);
        return loans;
    }

    public async Task<IEnumerable<Loan>> GetAsync(Borrower borrower)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<Loan> loans = await context.Loans.Include(l => l.Installments).Where(l => l.Borrower.Id == borrower.Id).ToListAsync();
        return loans;
    }

    public async Task UpdateAsync(Loan loan)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Loans.Update(loan);
        await context.SaveChangesAsync();
    }
}
