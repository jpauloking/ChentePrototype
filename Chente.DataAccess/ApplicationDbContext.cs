using Chente.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Chente.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Borrower> Borrowers { get; set; } = default!;
    public DbSet<Loan> Loans { get; set; } = default!;
    public DbSet<Installment> Installments { get; set; } = default!;

}
