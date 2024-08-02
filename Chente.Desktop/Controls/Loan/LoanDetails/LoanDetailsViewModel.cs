using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class LoanDetailsViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;
    [ObservableProperty]
    private bool hasSelectedLoan;

    public LoanViewModel LoanViewModel => mapper.Map<LoanViewModel>(loanStoreService.SelectedLoan);

    public LoanDetailsViewModel(NavigationService navigationService, LoanStoreService loanStoreService, IMapper mapper)
    {
        this.navigationService = navigationService;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
    }

    [RelayCommand]
    private void ShowInstallments()
    {
        if (HasSelectedLoan)
        {
            navigationService.NavigateTo<InstallmentsViewModel>();
        }
        else
        {
            MessageBox.Show("Please select a loan and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void AdjustInstallments()
    {
        if (HasSelectedLoan)
        {
            MessageBox.Show("This action requires premium subscription. Please upgrade your subscription and try again.", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Please select a loan and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan loan)
    {
        HasSelectedLoan = loanStoreService.SelectedLoan is not null;
        OnPropertyChanged(nameof(HasSelectedLoan));
        OnPropertyChanged(nameof(LoanViewModel));
    }
}
