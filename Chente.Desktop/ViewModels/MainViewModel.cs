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
    private readonly UserStoreService userStoreService;
    [ObservableProperty]
    private string? username = Thread.CurrentPrincipal?.Identity?.Name;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NotShowNavigationBar))]
    private bool showNavigationBar;

    public bool NotShowNavigationBar => !ShowNavigationBar;

    public NavigationService NavigationService => navigationService;

    public MainViewModel(NavigationService navigationService, DataSummarizationService dataSummarizationService, AuthenticationService authenticationService, UserStoreService userStoreService)
    {
        this.navigationService = navigationService;
        this.dataSummarizationService = dataSummarizationService;
        this.userStoreService = userStoreService;
        this.navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        authenticationService.AuthenticationStateChanged += OnAuthenticationStateChanged;
        NavigateToInitialView();
    }

    private void OnAuthenticationStateChanged(object? sender, EventArgs e)
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
    private void NavigateToUsers()
    {
        navigationService.NavigateTo<UsersViewModel>();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        dataSummarizationService.Refresh(); // On navigating to dashboard Refresh the data summarizations.
        navigationService.NavigateTo<DashboardViewModel>();
    }

    [RelayCommand]
    private void LogOut()
    {
        MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to log out. Unsaved changes will be lost.", "System says", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
        if (messageBoxResult == MessageBoxResult.Yes)
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.NoPrincipal);
            Thread.CurrentPrincipal = null;
            NavigateToLogin();
        }
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

    private void NavigateToRegister()
    {
        navigationService.NavigateTo<RegisterViewModel>();
    }


    private void NavigateToInitialView()
    {
        if (navigationService.CurrentViewModel is null)
        {
            var users = userStoreService.Users;
            if (users.Any())
            {
                NavigateToLogin();
            }
            else
            {
                NavigateToRegister();
            }
        }
    }

    private void OnCurrentViewModelChanged(object? sender, EventArgs e)
    {
        bool isLoginView = navigationService?.CurrentViewModel?.GetType() == typeof(LoginViewModel);
        bool isRegisterView = navigationService?.CurrentViewModel?.GetType() == typeof(RegisterViewModel);
        bool isConfirmOwnerView = navigationService?.CurrentViewModel?.GetType() == typeof(ConfirmOwnerViewModel);
        ShowNavigationBar = !isLoginView && !isRegisterView && !isConfirmOwnerView;
    }
}
