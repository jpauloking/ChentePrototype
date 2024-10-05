using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class ConfirmOwnerViewModel : ViewModelBase
{
    private readonly NavigationService navigationService;
    private readonly UserStoreService userStoreService;
    [Required]
    [ObservableProperty]
    private string securityAnswer = null!;

    public string SecurityQuestion { get; private set; }

    public ConfirmOwnerViewModel(NavigationService navigationService, UserStoreService userStoreService)
    {
        this.navigationService = navigationService;
        this.userStoreService = userStoreService;
        SecurityQuestion = "What was the name of your pet?";
    }

    [RelayCommand]
    private async Task SubmitAnswer()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            return;
        }
        string correctAnswer = "Simba";
        var owner = userStoreService.Users.FirstOrDefault(user => user.Role == "OWNER");
        if (owner is not null)
        {
            if (string.Equals(correctAnswer, SecurityAnswer, StringComparison.InvariantCultureIgnoreCase))
            {
                await userStoreService.DeleteAsync(owner.EmailAddress);
            }
            else
            {
                // Todo - Log security activity and notify owner/ administrator.
                MessageBox.Show("Failed security check. Owner has been notified via email. If your are the owner please contact your system administrator.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
                SecurityAnswer = string.Empty;
                ClearErrors();
                return;
            }
        }
        SecurityAnswer = string.Empty;
        ClearErrors();
        navigationService.NavigateTo<RegisterViewModel>();
    }

    [RelayCommand]
    private void LogIn()
    {
        SecurityAnswer = string.Empty;
        ClearErrors();
        navigationService.NavigateTo<LoginViewModel>();
    }
}
