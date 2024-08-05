using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal class DatabaseSummaryViewModel : ViewModelBase
{
    private readonly DataSummarizationService dataSummarizationService;

    public DatabaseSummaryViewModel(DataSummarizationService dataSummarizationService)
    {
        this.dataSummarizationService = dataSummarizationService;
    }

    #region Borrower Summarization
    public int BorrowersCount => dataSummarizationService.Borrowers.Count();
    public decimal BorrowersAmount => LoanAmount;
    public int BorrowersWithPendingInstallmentsCount => LoansPendingCount;
    public decimal BorrowersWithPendingInstallmentsAmount => InstallmentsPendingAmount;
    public int BorrowersWithOverdueInstallmentsCount => LoansOverdueCount;
    public decimal BorrowersWithOverdueInstallmentsAmount => InstallmentsOverdueAmount;
    public int BorrowersWithCompleteLoansCount => dataSummarizationService.Loans.Where(l => l.IsPaid).Select(l => l.Borrower.BorrowerNumber).Distinct().Count();
    public decimal BorrowersWithCompleteLoansAmount => LoansCompleteAmount;
    public int BorrowersReturningCount => dataSummarizationService.Loans.GroupBy(l => l.Borrower.BorrowerNumber).Count(borrowerLoansGroup => borrowerLoansGroup.Count() > 1);
    public decimal BorrowersReturningAmount => dataSummarizationService.Loans.GroupBy(l => l.Borrower.BorrowerNumber).Where(borrowerLoansGroup => borrowerLoansGroup.Count() > 1).Sum(borrowerLoansGroup => borrowerLoansGroup.Sum(l => l.Amount));

    public List<CategorySummaryItem> BorrowerCategorySummary => [
        new ()
        {
            Category = "All borrowers",
            Number = BorrowersCount,
            Amount = BorrowersAmount
        },
        new ()
        {
            Category = "With pending installments",
            Number = BorrowersWithPendingInstallmentsCount,
            Amount = BorrowersWithPendingInstallmentsAmount
        },
        new()
        {
            Category = "With overdue installments",
            Number = BorrowersWithOverdueInstallmentsCount,
            Amount = BorrowersWithOverdueInstallmentsAmount
        },
        new()
        {
            Category = "With complete loans",
            Number = BorrowersWithCompleteLoansCount,
            Amount = BorrowersWithCompleteLoansAmount
        },
        new()
        {
            Category = "Returning borrowers",
            Number = BorrowersReturningCount,
            Amount = BorrowersReturningAmount
        },
    ];
    #endregion Borrower Summarization

    #region Loan Summarization
    public int LoanCount => dataSummarizationService.Loans.Count();
    public decimal LoanAmount => dataSummarizationService.Loans.Sum(l => l.Amount);
    public int LoansPendingCount => dataSummarizationService.Loans.Where(l => !l.IsPaid).Count();
    public decimal LoansPendingAmount => dataSummarizationService.Loans.Where(l => !l.IsPaid).Sum(l => l.Amount);
    public int LoansCompleteCount => dataSummarizationService.Loans.Where(l => l.IsPaid).Count();
    public decimal LoansCompleteAmount => dataSummarizationService.Loans.Where(l => l.IsPaid).Sum(l => l.Amount);
    public int LoansOverdueCount => dataSummarizationService.Loans.Where(l => l.IsOverDue).Count();
    public decimal LoansOverdueAmount => dataSummarizationService.Loans.Where(l => l.IsOverDue).Sum(l => l.Amount);
    public int LoansPartiallyPaidCount => dataSummarizationService.Loans.Where(l => !l.IsPaid && l.Amount != l.AmountDue).Count();
    public decimal LoansPartiallyPaidAmountPaid => dataSummarizationService.Loans.Where(l => !l.IsPaid && l.Amount != l.AmountDue).Sum(l => (l.Amount - l.AmountDue));
    public decimal LoansPartiallyPaidAmountDue => dataSummarizationService.Loans.Where(l => !l.IsPaid && l.Amount != l.AmountDue).Sum(l => l.AmountDue);
    public List<CategorySummaryItem> LoanCategorySummary => [
        new ()
        {
            Category = "All loans",
            Number = LoanCount,
            Amount = LoanAmount
        },
        new ()
        {
            Category = "Loans pending",
            Number = LoansPendingCount,
            Amount = LoansPendingAmount
        },
        new()
        {
            Category = "Loans complete",
            Number = LoansCompleteCount,
            Amount = LoansCompleteAmount
        },
        new()
        {
            Category = "Loans partially paid (Amount paid)",
            Number = LoansPartiallyPaidCount,
            Amount = LoansPartiallyPaidAmountPaid
        },
        new()
        {
            Category = "Loans partially paid (Amount due)",
            Number = LoansPartiallyPaidCount,
            Amount = LoansPartiallyPaidAmountDue
        },
        new()
        {
            Category = "Loans overdue",
            Number = LoansOverdueCount,
            Amount = LoansOverdueAmount
        },
    ];
    #endregion Loan Summarization

    #region Installment Summarization
    public int InstallmentCount => dataSummarizationService.Installments.Count();
    public decimal InstallmentAmount => dataSummarizationService.Installments.Sum(i => i.Amount);
    public int InstallmentsPendingCount => dataSummarizationService.Installments.Where(i => !i.IsPaid).Count();
    public decimal InstallmentsPendingAmount => dataSummarizationService.Installments.Where(i => !i.IsPaid).Sum(l => l.Amount);
    public int InstallmentsCompleteCount => dataSummarizationService.Installments.Where(i => i.IsPaid).Count();
    public decimal InstallmentsCompleteAmount => dataSummarizationService.Installments.Where(i => i.IsPaid).Sum(l => l.Amount);
    public int InstallmentsOverdueCount => dataSummarizationService.Installments.Where(i => i.IsOverDue).Count();
    public decimal InstallmentsOverdueAmount => dataSummarizationService.Installments.Where(i => i.IsOverDue).Sum(l => l.Amount);
    public int InstallmentsPartiallyPaidCount => dataSummarizationService.Installments.Where(l => !l.IsPaid && l.Amount != l.AmountDue).Count();
    public decimal InstallmentsPartiallyPaidAmountPaid => dataSummarizationService.Installments.Where(l => !l.IsPaid && l.Amount != l.AmountDue).Sum(l => (l.Amount - l.AmountDue));
    public decimal InstallmentsPartiallyPaidAmountDue => dataSummarizationService.Installments.Where(l => !l.IsPaid && l.Amount != l.AmountDue).Sum(l => l.AmountDue);
    public List<CategorySummaryItem> InstallmentCategorySummary => [
        new ()
        {
            Category = "All installments",
            Number = InstallmentCount,
            Amount = InstallmentAmount
        },
        new ()
        {
            Category = "Installments pending",
            Number = InstallmentsPendingCount,
            Amount = InstallmentsPendingAmount
        },
        new()
        {
            Category = "Installments complete",
            Number = InstallmentsCompleteCount,
            Amount = InstallmentsCompleteAmount
        },
        new()
        {
            Category = "Installments partially paid (Amount paid)",
            Number = InstallmentsPartiallyPaidCount,
            Amount = InstallmentsPartiallyPaidAmountPaid
        },
        new()
        {
            Category = "Installments partially paid (Amount due)",
            Number = InstallmentsPartiallyPaidCount,
            Amount = InstallmentsPartiallyPaidAmountDue
        },
        new()
        {
            Category = "Installments overdue",
            Number = InstallmentsOverdueCount,
            Amount = InstallmentsOverdueAmount
        },
    ];
    #endregion Installment Summarization
}
