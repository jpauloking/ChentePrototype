using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowerDetailsViewModel : ViewModelBase
{
    private readonly ModalNavigationService modalNavigationService;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly WindowManager windowManager;
    private readonly IMapper mapper;

    //[ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(HasSelectedBorrower))]
    //[NotifyCanExecuteChangedFor(nameof(AddNewLoanCommand))]
    //private BorrowerViewModel? borrowerViewModel;

    //public bool HasSelectedBorrower => BorrowerViewModel is not null;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddNewLoanCommand))]
    private bool hasSelectedBorrower;

    public BorrowerViewModel BorrowerViewModel => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

    public BorrowerDetailsViewModel(ModalNavigationService modalNavigationService, WindowManager windowManager, BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.modalNavigationService = modalNavigationService;
        this.windowManager = windowManager;
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    [RelayCommand]
    private void ShowLoans()
    {
        if (HasSelectedBorrower)
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
        if (HasSelectedBorrower)
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
        if (HasSelectedBorrower)
        {
            await borrowerStoreService.DeleteAsync(DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrowerStoreService.SelectedBorrower!.BorrowerNumber));
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    private bool CanAddNewLoan()
    {
        return HasSelectedBorrower;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower borrower)
    {
        HasSelectedBorrower = borrowerStoreService.SelectedBorrower is not null;
        HasSelectedBorrower = BorrowerViewModel is not null;
        OnPropertyChanged(nameof(HasSelectedBorrower));
        OnPropertyChanged(nameof(BorrowerViewModel));
    }

}
