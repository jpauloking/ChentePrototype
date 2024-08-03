using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowerDetailsViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;

    [ObservableProperty]
    private bool hasSelectedBorrower;

    public BorrowerViewModel BorrowerViewModel => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

    public BorrowerDetailsViewModel(NavigationService navigationService, BorrowerStoreService borrowerStoreService, LoanStoreService loanStoreService, IMapper mapper)
    {
        this.navigationService = navigationService;
        this.borrowerStoreService = borrowerStoreService;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    [RelayCommand]
    private void ShowLoans()
    {
        if (HasSelectedBorrower)
        {
            if (loanStoreService.Loans.Count() > 0)
            {
                navigationService.NavigateTo<LoansViewModel>();
            }
            else
            {
                MessageBox.Show("Please add atleast one loan to this borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please add atleast one loan to this borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void AddNewLoan()
    {
        if (HasSelectedBorrower)
        {
            navigationService.NavigateTo<LoansViewModel>();
            LoansViewModel? loansViewModel = (navigationService.CurrentViewModel as LoansViewModel);
            if (loansViewModel is not null)
            {
                loansViewModel.LoanFormViewModel.ShowLoanForm = true;
            }
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower borrower)
    {
        HasSelectedBorrower = borrowerStoreService.SelectedBorrower is not null;
        OnPropertyChanged(nameof(HasSelectedBorrower));
        OnPropertyChanged(nameof(BorrowerViewModel));
    }

}
