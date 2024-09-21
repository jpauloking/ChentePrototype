using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Claims;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    private readonly DataSummarizationService dataSummarizationService;
    [ObservableProperty]
    private string? username = Thread.CurrentPrincipal?.Identity?.Name;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NotShowNavigationBar))]
    private bool showNavigationBar;

    public bool NotShowNavigationBar => !ShowNavigationBar;

    public NavigationService NavigationService => navigationService;

    public MainViewModel(NavigationService navigationService, DataSummarizationService dataSummarizationService)
    {
        this.navigationService = navigationService;
        this.dataSummarizationService = dataSummarizationService;
        this.navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        ChenteIdentityProvider.IdentityChanged += OnIdentityChanged;
        NavigateToInitialView();
    }

    private void OnIdentityChanged(object? sender, EventArgs e)
    {
        Username = (((System.Security.Principal.GenericIdentity?)Thread.CurrentPrincipal?.Identity)?.FindFirst(claim => claim.Type == ClaimTypes.Name && claim.Issuer == "Chente"))?.Value;
        OnPropertyChanged(nameof(Username));
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
        dataSummarizationService.Refresh(); // On navigating to dashboard Refresh the data summarizations.
        navigationService.NavigateTo<DashboardViewModel>();
    }

    [RelayCommand]
    private void Shutdown()
    {
        MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to shutdown. Unsaved changes will be lost.", "System says", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
        if (messageBoxResult == MessageBoxResult.Yes)
        {
            App.Current.Shutdown();
        }
    }

    private void NavigateToLogin()
    {
        navigationService.NavigateTo<LoginViewModel>();
    }

    private void NavigateToInitialView()
    {
        if (navigationService.CurrentViewModel is null)
        {
            NavigateToLogin();
        }
    }

    private void OnCurrentViewModelChanged(object? sender, EventArgs e)
    {
        bool isLoginView = navigationService?.CurrentViewModel?.GetType() == typeof(LoginViewModel);
        ShowNavigationBar = !isLoginView;
    }
}
