using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class RegisterViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly UserStoreService userStoreService;
    private readonly NavigationService navigationService;

    [Required]
    [MaxLength(50)]
    [ObservableProperty]
    private string role = null!;

    [Required]
    [MaxLength(50)]
    [ObservableProperty]
    private string username = null!;

    [EmailAddress]
    [Required]
    [ObservableProperty]
    private string emailAddress = null!;

    [Phone]
    [ObservableProperty]
    private string? phoneNumber;

    [Required]
    [ObservableProperty]
    private string password = null!;

    [Required]
    //[Compare(nameof(Password))]
    [ObservableProperty]
    private string confirmPassword = null!;

    [RelayCommand]
    private async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Password and confirm password do not match. Check passwords and try again.");
                return;
            }
            UserViewModel userViewModel = new()
            {
                Role = Role,
                Username = Username,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
            };

            DataAccess.Models.User user = mapper.Map<DataAccess.Models.User>(userViewModel);
            await userStoreService.CreateAsync(user);
            NavigateToLogIn();
        }
    }

    [RelayCommand]
    private void NavigateToLogIn()
    {
        Role = "OWNER";
        Username = string.Empty;
        EmailAddress = string.Empty;
        PhoneNumber = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        ClearErrors();
        navigationService.NavigateTo<LoginViewModel>();
    }

    public RegisterViewModel(UserStoreService userStoreService, NavigationService navigationService, IMapper mapper)
    {
        this.userStoreService = userStoreService;
        this.mapper = mapper;
        this.navigationService = navigationService;
        Role = "OWNER";
    }
}
