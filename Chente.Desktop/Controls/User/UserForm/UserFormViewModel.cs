using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class UserFormViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly UserStoreService userStoreService;

    public UserViewModel SelectedUser => mapper.Map<UserViewModel>(userStoreService.SelectedUser);

    [ObservableProperty]
    private bool showUserForm;

    [Required]
    [MaxLength(50)]
    [ObservableProperty]
    private string role = "TELLER";

    [Required]
    [MaxLength(50)]
    [ObservableProperty]
    private string username = null!;

    [EmailAddress]
    [Required]
    [ObservableProperty]
    public string emailAddress = null!;

    [Phone]
    [ObservableProperty]
    public string? phoneNumber;

    [Required]
    [ObservableProperty]
    public string password = null!;

    [Required]
    [ObservableProperty]
    public string confirmPassword = null!;

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
                Password = Password,
            };

            DataAccess.Models.User user = mapper.Map<DataAccess.Models.User>(userViewModel);
            if (SelectedUser is not null)
            {
                await userStoreService.UpdateAsync(user);
            }
            else
            {
                await userStoreService.CreateAsync(user);
            }

            ShowUserForm = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        ShowUserForm = false;
        Username = string.Empty;
        Role = "TELLER";
        EmailAddress = string.Empty;
        PhoneNumber = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        OnPropertyChanged(nameof(SelectedUser));
        ClearErrors();
    }

    public UserFormViewModel(UserStoreService userStoreService, IMapper mapper)
    {
        this.userStoreService = userStoreService;
        this.mapper = mapper;
        this.userStoreService.SelectedUserChanged += OnSelectedUserChanged;
        this.userStoreService.UsersCollectionChanged += OnUsersCollectionChanged; ;
        if (SelectedUser is not null)
        {
            Username = SelectedUser.Username;
            Role = SelectedUser.Role;
            EmailAddress = SelectedUser.EmailAddress;
            PhoneNumber = SelectedUser.PhoneNumber;
        }
    }

    private void OnUsersCollectionChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(SelectedUser));
        ClearErrors();
    }

    private void OnSelectedUserChanged(object? sender, EventArgs args)
    {
        if (SelectedUser is not null)
        {
            Username = SelectedUser.Username;
            Role = SelectedUser.Role;
            EmailAddress = SelectedUser.EmailAddress;
            PhoneNumber = SelectedUser.PhoneNumber;
        }
        else
        {
            Username = string.Empty;
            Role = "TELLER";
            EmailAddress = string.Empty;
            PhoneNumber = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            OnPropertyChanged(nameof(SelectedUser));
        }
        ClearErrors();
    }
}
