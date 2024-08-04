using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class LoanViewModel : ViewModelBase
{
    [ObservableProperty]
    private string loanNumber = null!;
    [ObservableProperty]
    private DateTime dateOpened;
    [ObservableProperty]
    private decimal principal;
    [ObservableProperty]
    private double interestRate;
    [ObservableProperty]
    private int durationInDays;
    [ObservableProperty]
    private decimal amountPerInstallment;
    [ObservableProperty]
    private BorrowerViewModel borrower = null!;
    [ObservableProperty]
    private int numberOfInstallments;
    [ObservableProperty]
    private decimal amount;
    [ObservableProperty]
    private decimal amountDue;
    [ObservableProperty]
    private bool isPaid;
    [ObservableProperty]
    private DateTime? datePaid;
    [ObservableProperty]
    private bool isOverDue;
    [ObservableProperty]
    private int numberOfInstallmentsOverDue;
}
