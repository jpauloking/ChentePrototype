using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class LoanDetailsViewModel : ViewModelBase
{
    private readonly ModalNavigationService modalNavigationService;
    private readonly LoanStoreService loanStoreService;
    private readonly WindowManager windowManager;
    private readonly IMapper mapper;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddNewLoanCommand))]
    private bool hasSelectedLoan;

    public LoanViewModel LoanViewModel => mapper.Map<LoanViewModel>(loanStoreService.SelectedLoan);

    public LoanDetailsViewModel(ModalNavigationService modalNavigationService, WindowManager windowManager, LoanStoreService loanStoreService, IMapper mapper)
    {
        this.modalNavigationService = modalNavigationService;
        this.windowManager = windowManager;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
    }

    [RelayCommand]
    private void ShowLoans()
    {
        if (HasSelectedLoan)
        {
            modalNavigationService.NavigateTo<LoanListViewModel>();
            windowManager.ShowModal<ModalViewModel>();
        }
        else
        {
            MessageBox.Show("Please add atleast one loan to this borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanAddNewLoan))]
    private void AddNewLoan()
    {
        if (HasSelectedLoan)
        {
            modalNavigationService.NavigateTo<LoanFormViewModel>();
            windowManager.ShowModal<ModalViewModel>();
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task DeleteBorrower()
    {
        if (HasSelectedLoan)
        {
            await loanStoreService.DeleteAsync(DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(loanStoreService.SelectedLoan!.LoanNumber));
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    private bool CanAddNewLoan()
    {
        return HasSelectedLoan;
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan loan)
    {
        HasSelectedLoan = loanStoreService.SelectedLoan is not null;
        HasSelectedLoan = LoanViewModel is not null;
        OnPropertyChanged(nameof(HasSelectedLoan));
        OnPropertyChanged(nameof(LoanViewModel));
    }
}
