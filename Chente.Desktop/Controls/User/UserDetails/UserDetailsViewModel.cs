using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class UserDetailsViewModel : ViewModelBase
{
    private readonly UserStoreService userStoreService;
    private readonly IMapper mapper;

    [ObservableProperty]
    private bool hasSelectedUser;

    public UserViewModel UserViewModel => mapper.Map<UserViewModel>(userStoreService.SelectedUser);

    public UserDetailsViewModel(UserStoreService userStoreService, IMapper mapper)
    {
        this.userStoreService = userStoreService;
        this.mapper = mapper;
        this.userStoreService.SelectedUserChanged += OnSelectedUserChanged;
    }

    private void OnSelectedUserChanged(object? sender, EventArgs args)
    {
        HasSelectedUser = userStoreService.SelectedUser is not null;
        OnPropertyChanged(nameof(HasSelectedUser));
        OnPropertyChanged(nameof(UserViewModel));
    }

    //[RelayCommand]
    //private void ShowLoans()
    //{
    //    if (HasSelectedBorrower)
    //    {
    //        if (loanStoreService.Loans.Count() > 0)
    //        {
    //            navigationService.NavigateTo<LoansViewModel>();
    //        }
    //        else
    //        {
    //            MessageBox.Show("Please add atleast one loan to this borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
    //        }
    //    }
    //    else
    //    {
    //        MessageBox.Show("Please add atleast one loan to this borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
    //    }
    //}

    //[RelayCommand]
    //private void AddNewLoan()
    //{
    //    if (HasSelectedBorrower)
    //    {
    //        navigationService.NavigateTo<LoansViewModel>();
    //        LoansViewModel? loansViewModel = (navigationService.CurrentViewModel as LoansViewModel);
    //        if (loansViewModel is not null)
    //        {
    //            loansViewModel.LoanFormViewModel.ShowLoanForm = true;
    //        }
    //    }
    //    else
    //    {
    //        MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
    //    }
    //}

}
