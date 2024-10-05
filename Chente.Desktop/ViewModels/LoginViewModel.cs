using Chente.Desktop.Core;
using Chente.Desktop.Exceptions;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    private readonly AuthenticationService authenticationService;
    private readonly UserStoreService userStoreService;

    [ObservableProperty]
    [Required]
    [EmailAddress]
    private string emailAddress = null!;
    [Required]
    [ObservableProperty]
    private string password = null!;

    public LoginViewModel(NavigationService navigationService, AuthenticationService authenticationService, UserStoreService userStoreService)
    {
        this.navigationService = navigationService;
        this.authenticationService = authenticationService;
        this.userStoreService = userStoreService;
    }

    [RelayCommand]
    private async Task LogInAsync()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            try
            {
                var principal = (GenericPrincipal?)(await authenticationService.LogIn(EmailAddress, Password));
                Thread.CurrentPrincipal = principal;
                if (principal is null)
                {
                    MessageBox.Show("Invalid credentials. Log in failed", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
                    Password = string.Empty;
                    return;
                }
                var identity = (GenericIdentity?)principal!.Identity;
                if (identity is null)
                {
                    MessageBox.Show("Invalid credentials. Log in failed", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
                    Password = string.Empty;
                    return;
                }
                if (identity!.IsAuthenticated)
                {
                    EmailAddress = string.Empty;
                    Password = string.Empty;
                    navigationService.NavigateTo<DashboardViewModel>();
                }
            }
            catch (AuthenticationFailedException e)
            {
                MessageBox.Show(e.Message, "System says", MessageBoxButton.OK, MessageBoxImage.Error);
                Password = string.Empty;
            }
        }
    }

    [RelayCommand]
    private void NavigateToRegister()
    {
        EmailAddress = string.Empty;
        Password = string.Empty;
        ClearErrors();
        var owner = userStoreService.Users.FirstOrDefault(user => user.Role == "OWNER");
        if (owner is not null)
        {
            // Todo - Verify that the user is owner of the application before resetting credentials e.g. ask security question.
            // await userStoreService.DeleteAsync(owner.EmailAddress);
            navigationService.NavigateTo<ConfirmOwnerViewModel>();
        }
        else
        {
            MessageBox.Show("Cannot reset security credentials. Please contact your system administrator.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ShutDown()
    {
        App.Current.Shutdown();
    }
}
