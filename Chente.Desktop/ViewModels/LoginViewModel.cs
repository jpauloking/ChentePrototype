using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    [ObservableProperty]
    [Required]
    [EmailAddress]
    private string username = null!;
    [Required]
    [ObservableProperty]
    private string password = null!;

    public LoginViewModel(NavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    [RelayCommand]
    private void LogIn()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            ChenteIdentityProvider.AddClaim(ClaimTypes.Name, Username);
            ChenteIdentityProvider.AddClaim(ClaimTypes.Role, "TELLER");
            var principal = (GenericPrincipal?)Thread.CurrentPrincipal;
            if (principal is null)
            {
                MessageBox.Show("Log in failed", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var identity = (GenericIdentity?)principal!.Identity;
            if (identity is null)
            {
                MessageBox.Show("Log in failed", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (identity!.IsAuthenticated)
            {
                navigationService.NavigateTo<DashboardViewModel>();
            }
        }
    }

    [RelayCommand]
    private void ShutDown()
    {
        App.Current.Shutdown();
    }
}
