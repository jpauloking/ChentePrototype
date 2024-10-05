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
    public DbSet<User> Users { get; set; } = default!;

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<User>().HasData(new User()
    //    {
    //        Id = 1,
    //        EmailAddress = "admin@gmail.com",
    //        Password = BCrypt.Net.BCrypt.HashPassword("p#55w0RD"),
    //        PhoneNumber = "+256-777-000-000",
    //        Role = "CHENTE",
    //        Username = "admin"
    //    });
    //    base.OnModelCreating(modelBuilder);
    //}
}
