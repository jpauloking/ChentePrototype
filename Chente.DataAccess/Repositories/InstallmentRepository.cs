using Chente.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Chente.DataAccess.Repositories;

public class InstallmentRepository
{
    private readonly ApplicationDbContextFactory contextFactory;

    public InstallmentRepository(ApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<IEnumerable<Installment>> GetAsync()
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<Installment> installments = await context.Installments.Include(i => i.Loan).ToListAsync();
        return installments;
    }

    public async Task<IEnumerable<Installment>> GetAsync(Loan loan)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<Installment> installments = await context.Installments.Include(i => i.Loan)
            .Where(i => i.Loan.Id == loan.Id).ToListAsync();
        return installments;
    }

    public async Task<IEnumerable<Installment>> GetAsync(Borrower borrower)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        List<Installment> installments = await context.Installments.Include(i => i.Loan)
            .Where(i => i.Loan.Borrower.Id == borrower.Id).ToListAsync();
        return installments;
    }

    public async Task<Installment?> GetAsync(int id)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        Installment? installment = await context.Installments.Include(i => i.Loan).FirstOrDefaultAsync(i => i.Id == id);
        return installment;
    }

    public async Task UpdateAsync(Installment installment)
    {
        using ApplicationDbContext context = contextFactory.CreateDbContext();
        context.Installments.Update(installment);
        await context.SaveChangesAsync();
    }
}
