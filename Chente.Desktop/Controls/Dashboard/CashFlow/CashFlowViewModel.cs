using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal class CashFlowViewModel : ViewModelBase
{
    private readonly DataSummarizationService dataSummarizationService;

    public CashFlowViewModel(DataSummarizationService dataSummarizationService)
    {
        this.dataSummarizationService = dataSummarizationService;
    }

    public decimal LoansGivenOut => dataSummarizationService.Loans.Sum(loan => loan.Principal);
    public decimal LoansRecovered => dataSummarizationService.Loans.Where(loan => loan.IsPaid).Sum(loan => loan.Principal);
    public decimal LoansPending => dataSummarizationService.Loans.Where(loan => !loan.IsPaid).Sum(loan => loan.Principal);

    public decimal AmountGivenOut => dataSummarizationService.Loans.Sum(loan => loan.Amount);
    public decimal AmountRecovered => dataSummarizationService.Loans.Where(loan => loan.IsPaid).Sum(loan => loan.Amount);
    public decimal AmountPending => dataSummarizationService.Loans.Where(loan => !loan.IsPaid).Sum(loan => loan.Amount);

    public decimal InterestEarned => AmountGivenOut - LoansGivenOut;
    public decimal InterestPending => AmountPending - LoansPending;
    public decimal InterestRecovered => InterestEarned - InterestPending;

    public decimal InstallmentsRecovered => dataSummarizationService.Installments.Sum(installment => installment.AmountPaid);
    public decimal InstallmentsFullyRecovered => dataSummarizationService.Installments.Where(installment => installment.IsPaid).Sum(installment => installment.Amount);
    public decimal InstallmentsPending => dataSummarizationService.Installments.Where(installment => !installment.IsPaid).Sum(installment => installment.AmountDue);
    public decimal InstallmentsOverdue => dataSummarizationService.Installments.Where(installment => !installment.IsOverDue).Sum(installment => installment.AmountDue);

}
