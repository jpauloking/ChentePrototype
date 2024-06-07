using Chente.Console.Modules;
using Chente.DataAccess;
using Chente.Domain.Models;
using static System.Console;

DisplayBrand();
await CreateDatabase();

ApplicationDbContextFactory applicationDbContextFactory = new();

//Borrower firstBorrower = new ("John Paul", "Obongo", "johnpaul@email.mail", "+256 787 728 635");
//Borrower secondBorrower = new ("Jude", "Obongo", "jude@email.mail", "+256 787 728 635");
//Borrower thirdBorrower = new ("Emmanuel", "Obong", "emmanuel@email.mail", "+256 787 728 635");
//Borrower forthBorrower = new ("Moses", "Obua", "moses@email.mail", "+256 787 728 635");

BorrowerModule borrowerModule = new(applicationDbContextFactory);
LoanModule loanModule = new(applicationDbContextFactory);
InstallmentModule installmentModule = new(applicationDbContextFactory);

/*
 * Create borrowers
 */

//await borrowerModule.CreateBorrower(firstBorrower);
//await borrowerModule.CreateBorrower(secondBorrower);
//await borrowerModule.CreateBorrower(thirdBorrower);
//await borrowerModule.CreateBorrower(forthBorrower);

/*
 * Display all borrowers
 */

//var borrowers = await borrowerModule.GetBorrowers();
//foreach (var borrower in borrowers)
//{
//    borrowerModule.DisplayBorrower(borrower);
//}

/*
 * Borrower with ID == 1
 */

Borrower borrowerWithIDEqualToOne = await borrowerModule.GetBorrower(1);
borrowerModule.DisplayBorrower(borrowerWithIDEqualToOne);

//await borrowerModule.AddLoanToBorrower(borrowerWithIDEqualToOne, new Loan(DateTime.Today, 10_000M, 10, 30, 5_000M));

//Installment firstInstallment = await installmentModule.GetInstallment(3);
//installmentModule.PayInstallment(firstInstallment, 5_000, DateTime.Today);

//Installment secondInstallment = await installmentModule.GetInstallment(4);
//installmentModule.PayInstallment(secondInstallment, 3_000, DateTime.Today);

//Installment secondInstallment = await installmentModule.GetInstallment(4);
//installmentModule.PayInstallment(secondInstallment, 2_000, DateTime.Today);

//Installment thirdInstallment = await installmentModule.GetInstallment(5);
//installmentModule.PayInstallment(thirdInstallment, 1_000, DateTime.Today);

//await borrowerModule.AddLoanToBorrower(borrowerWithIDEqualToOne, new Loan(DateTime.Today, 10_000M, 10, 30, 10_000M));

//Installment firstInstallment = await installmentModule.GetInstallment(1);
//installmentModule.PayInstallment(firstInstallment, 10_000, DateTime.Today);

//Installment secondInstallment = await installmentModule.GetInstallment(2);
//installmentModule.PayInstallment(secondInstallment, 1_000, DateTime.Today);

//await borrowerModule.AddLoanToBorrower(borrowerWithIDEqualToOne, new Loan(DateTime.Today, 50_000M, 10, 30, 25_000M));

//Installment firstInstallment = await installmentModule.GetInstallment(6);
//installmentModule.PayInstallment(firstInstallment, 30_000, DateTime.Today);

//Installment secondInstallment = await installmentModule.GetInstallment(7);
//installmentModule.PayInstallment(secondInstallment, 40_000, DateTime.Today);

//Installment thirdInstallment = await installmentModule.GetInstallment(8);
//installmentModule.PayInstallment(thirdInstallment, 5_000, DateTime.Today);

var loansForBorrowerWithIDEqualToOne = await loanModule.GetLoans(borrowerWithIDEqualToOne);
foreach (var loan in loansForBorrowerWithIDEqualToOne)
{
    loanModule.DisplayLoan(loan);
    var installments = (await installmentModule.GetInstallments(loan)).ToList();
    installmentModule.DisplayLoanInstallments(installments);
}

//await borrowerModule.AddLoanToBorrower(borrower, new Loan(DateTime.Today, 200_000M, 10, 30, 50_000M));

//Installment firstInstallment = await installmentModule.GetInstallment(1);
//installmentModule.PayInstallment(firstInstallment, 50_000, DateTime.Today);

//Installment secondInstallment = await installmentModule.GetInstallment(2);
//installmentModule.PayInstallment(secondInstallment, 50_000, DateTime.Today);

//Installment thirdInstallment = await installmentModule.GetInstallment(3);
//installmentModule.PayInstallment(thirdInstallment, 50_000, DateTime.Today);

//Installment forthInstallment = await installmentModule.GetInstallment(4);
//installmentModule.PayInstallment(forthInstallment, 50_000, DateTime.Today);

//Installment fifthInstallment = await installmentModule.GetInstallment(5);
//installmentModule.PayInstallment(fifthInstallment, 20_000, DateTime.Today);

void DisplayBrand()
{
    Title = "Chente";
    Clear();
    WriteLine("0000\t0  0\t0000\t0000\t00000\t0000");
    WriteLine("0   \t0  0\t0  0\t0  0\t  0  \t0  0");
    WriteLine("0   \t0  0\t0  0\t0  0\t  0  \t0  0");
    WriteLine("0   \t0000\t0000\t0  0\t  0  \t0000");
    WriteLine("0   \t0  0\t0   \t0  0\t  0  \t0   ");
    WriteLine("0   \t0  0\t0   \t0  0\t  0  \t0   ");
    WriteLine("0000\t0  0\t0000\t0  0\t  0  \t0000");
}

async Task CreateDatabase()
{
    ApplicationDbContextFactory applicationDbContextFactory = new ApplicationDbContextFactory();
    using ApplicationDbContext context = applicationDbContextFactory.CreateDbContext();
    await context.Database.EnsureCreatedAsync();
}

ReadKey();