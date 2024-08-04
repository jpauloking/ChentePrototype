using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

public partial class InstallmentViewModel : ViewModelBase
{
    [ObservableProperty]
    private string installmentNumber = null!;
    [ObservableProperty]
    private DateTime dateDue;
    [ObservableProperty]
    private decimal amount;
    [ObservableProperty]
    private decimal beginningBalance;
    [ObservableProperty]
    private decimal endingBalance;
    [ObservableProperty]
    private decimal amountPaid;
    [ObservableProperty]
    private DateTime datePaid;
    [ObservableProperty]
    private decimal amountDue;
    [ObservableProperty]
    private string loanNumber = null!;
    [ObservableProperty]
    public bool isPaid = false;
    [ObservableProperty]
    public bool isOverDue = false;
    [ObservableProperty]
    public int daysIsOverDueBy = 0;
}
