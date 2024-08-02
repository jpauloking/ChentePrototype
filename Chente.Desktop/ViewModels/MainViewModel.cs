using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.Input;

namespace Chente.Desktop.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;

    public NavigationService NavigationService => navigationService;

    public MainViewModel(NavigationService navigationService)
    {
        this.navigationService = navigationService;
        this.navigationService.CurrentViewModelChanged += HandleCurrentViewModelChanged;
        NavigateToInitialView();
    }

    [RelayCommand]
    private void NavigateToBorrowers()
    {
        navigationService.NavigateTo<BorrowersViewModel>();
    }

    [RelayCommand]
    private void NavigateToLoans()
    {
        navigationService.NavigateTo<LoansViewModel>();
    }

    [RelayCommand]
    private void NavigateToInstallments()
    {
        navigationService.NavigateTo<InstallmentsViewModel>();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        navigationService.NavigateTo<DashboardViewModel>();
    }

    private void HandleCurrentViewModelChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged();
    }

    private void NavigateToInitialView()
    {
        if (navigationService.CurrentViewModel is null)
        {
            NavigateToDashboard();
        }
    }
}
