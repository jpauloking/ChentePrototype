using AutoMapper;
using Chente.Desktop.Controls.Loan.LoanList;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class LoansViewModel : ViewModelBase
{
    private readonly DateTime today = DateTime.Now;
    private readonly IMapper mapper;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly LoanStoreService loanStoreService;
    private readonly LoanListViewModel loanListViewModel;
    private readonly LoanDetailsViewModel loanDetailsViewModel;
    private readonly LoanFormViewModel loanFormViewModel;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddLoanCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteLoanCommand))]
    private bool hasSelectedLoan = false;
    [ObservableProperty]
    private string pageTitle = "All loans";

    public DateTime DisplayDateStart => today.AddDays(-365);
    public DateTime DisplayDate => today;
    public DateTime DisplayDateEnd => today.AddDays(365);
    public LoanListViewModel LoanListViewModel => loanListViewModel;
    public LoanDetailsViewModel LoanDetailsViewModel => loanDetailsViewModel;
    public LoanFormViewModel LoanFormViewModel => loanFormViewModel;
    public IEnumerable<BorrowerViewModel> Borrowers => mapper.Map<IEnumerable<BorrowerViewModel>>(borrowerStoreService.Borrowers);

    public string? SearchPhrase
    {
        get => loanStoreService.SearchPhrase;
        set => loanStoreService.SearchPhrase = value;
    }

    public BorrowerViewModel SelectedBorrower
    {
        get => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);
        set => borrowerStoreService.SelectedBorrower = mapper.Map<Domain.Models.Borrower>(value);
    }

    public DateTime? StartDate
    {
        get => loanStoreService.StartDate;
        set => loanStoreService.StartDate = value;
    }

    public DateTime? EndDate
    {
        get => loanStoreService.EndDate;
        set => loanStoreService.EndDate = value;
    }

    public bool IncludePaid
    {
        get => loanStoreService.IncludePaid;
        set => loanStoreService.IncludePaid = value;
    }

    public bool OnlyOverdue
    {
        get => loanStoreService.OnlyOverdue;
        set => loanStoreService.OnlyOverdue = value;
    }

    public LoansViewModel(IMapper mapper, BorrowerStoreService borrowerStoreService, LoanDetailsViewModel loanDetailsViewModel, LoanListViewModel loanListViewModel, LoanStoreService loanStoreService, LoanFormViewModel loanFormViewModel)
    {
        this.mapper = mapper;
        this.borrowerStoreService = borrowerStoreService;
        this.loanDetailsViewModel = loanDetailsViewModel;
        this.loanListViewModel = loanListViewModel;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
        this.loanFormViewModel = loanFormViewModel;

        PageTitle = borrowerStoreService.SelectedBorrower is not null ? $"{SelectedBorrower?.DisplayName}'s loans" : "All loans";
        this.loanStoreService = loanStoreService;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        HasSelectedLoan = loanStoreService.SelectedLoan is not null;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        PageTitle = SelectedBorrower is not null ? $"{SelectedBorrower.DisplayName}'s loans" : "All loans";
        OnPropertyChanged(nameof(PageTitle));
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnNewLoan))]
    private void AddLoan()
    {
        if (SelectedBorrower is not null)
        {
            if (loanStoreService.SelectedLoan is not null)
            {
                loanStoreService.SelectedLoan = null;
            }
            LoanFormViewModel.ShowLoanForm = true;
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnSelectedLoan))]
    private async Task DeleteLoan()
    {
        if (loanStoreService.SelectedLoan is not null)
        {
            MessageBoxResult userResponse = MessageBox.Show($"Are you sure you want to delete loan? Loan Number: {loanStoreService.SelectedLoan.LoanNumber} will be deleted permanently. THIS ACTION IS NOT REVERSIBLE", "System caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (userResponse == MessageBoxResult.OK)
            {
                await loanStoreService.DeleteAsync(DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(loanStoreService.SelectedLoan!.LoanNumber));
            }
            else
            {
                MessageBox.Show("Action cancelled.", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("Please select a loan and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        StartDate = null!;
        EndDate = null!;
        IncludePaid = false;
        OnlyOverdue = false;
        SelectedBorrower = null!;
        OnPropertyChanged(nameof(StartDate));
        OnPropertyChanged(nameof(EndDate));
        OnPropertyChanged(nameof(IncludePaid));
        OnPropertyChanged(nameof(OnlyOverdue));
        OnPropertyChanged(nameof(SelectedBorrower));
        OnPropertyChanged(nameof(Borrowers));
        LoanFormViewModel.ShowLoanForm = false;
    }

    private bool CanExecuteOnSelectedLoan()
    {
        return HasSelectedLoan;
    }

    private bool CanExecuteOnNewLoan()
    {
        return !HasSelectedLoan;
    }
}
