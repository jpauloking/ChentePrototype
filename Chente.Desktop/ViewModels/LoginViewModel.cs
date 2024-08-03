using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
//using Windows.Security.Credentials;

namespace Chente.Desktop.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    [ObservableProperty]
    private string username = ChenteIdentityProvider.Username;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private bool isAuthenticated = ChenteIdentityProvider.IsLoggedIn;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private bool isGuest = ChenteIdentityProvider.IsGuest;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private bool isAnonymous = ChenteIdentityProvider.IsNotLoggedIn;

    public LoginViewModel(NavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteContinue))]
    private async Task Continue()
    {
        //if (await IsValidPassword())
        //{
            navigationService.NavigateTo<DashboardViewModel>();
        //}
    }

    private async Task<bool> IsValidPassword()
    {
        bool isValidFingerprint = false;
        //var supported = await KeyCredentialManager.IsSupportedAsync();

        //if (!supported)
        //{
        //    return isValidFingerprint;
        //}

        //var result = await KeyCredentialManager.RequestCreateAsync("login", KeyCredentialCreationOption.ReplaceExisting);

        //if (result.Status == KeyCredentialStatus.Success)
        //{
        //    isValidFingerprint = true;
        //}

        return isValidFingerprint;
    }

    [RelayCommand]
    private void ChangeAccount()
    {
        App.Current.Shutdown();
    }

    private bool CanExecuteContinue => IsAuthenticated && !IsAnonymous && !IsGuest;
}
