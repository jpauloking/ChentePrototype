using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class UsersViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly UserStoreService userStoreService;
    private readonly UserListViewModel usersListViewModel;
    private readonly UserDetailsViewModel userDetailsViewModel;
    private readonly UserFormViewModel userFormViewModel;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddUserCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditUserCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
    private bool hasSelectedUser = false;
    [ObservableProperty]
    private int selectedIndex;

    public UserListViewModel UserListViewModel => usersListViewModel;
    public UserDetailsViewModel UserDetailsViewModel => userDetailsViewModel;
    public UserFormViewModel UserFormViewModel => userFormViewModel;
    public IEnumerable<UserViewModel> Users => mapper.Map<IEnumerable<UserViewModel>>(userStoreService.Users);

    public string? SearchPhrase
    {
        get => userStoreService.SearchPhrase;
        set => userStoreService.SearchPhrase = value;
    }
    public UserViewModel? SelectedUser
    {
        get => mapper.Map<UserViewModel>(userStoreService.SelectedUser);
        set => userStoreService.SelectedUser = mapper.Map<DataAccess.Models.User>(value);
    }

    public UsersViewModel(IMapper mapper, UserStoreService userStoreService, UserListViewModel userListViewModel, UserFormViewModel userFormViewModel, UserDetailsViewModel userDetailsViewModel)
    {
        this.mapper = mapper;
        this.userStoreService = userStoreService;
        this.usersListViewModel = userListViewModel;
        this.userFormViewModel = userFormViewModel;
        this.userDetailsViewModel = userDetailsViewModel;
        this.userStoreService.SelectedUserChanged += OnSelectedUserChanged;
        this.userStoreService.UsersCollectionChanged += OnUsersCollectionChanged;
    }

    private void OnUsersCollectionChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Users));
        OnPropertyChanged(nameof(SelectedUser));
        SelectedIndex = Users.ToList().IndexOf(SelectedUser!);
        HasSelectedUser = SelectedUser is not null;
    }

    private void OnSelectedUserChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(SelectedUser));
        SelectedIndex = Users.ToList().IndexOf(SelectedUser!);
        HasSelectedUser = SelectedUser is not null;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnNewBorrower))]
    private void AddUser()
    {
        if (userStoreService.SelectedUser is not null)
        {
            userStoreService.SelectedUser = null;
        }
        UserFormViewModel.ShowUserForm = true;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnSelectedBorrower))]
    private void EditUser()
    {
        if (userStoreService.SelectedUser is not null)
        {
            UserFormViewModel.ShowUserForm = true;
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnSelectedBorrower))]
    private async Task DeleteUser()
    {
        if (SelectedUser is not null)
        {
            MessageBoxResult userResponse = MessageBox.Show($"Are you sure you want to delete user Username = [{SelectedUser.Username}] and Email address = [{SelectedUser.EmailAddress}]? THIS ACTION IS NOT REVERSIBLE", "System caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (userResponse == MessageBoxResult.OK)
            {
                await userStoreService.DeleteAsync(userStoreService.SelectedUser!.EmailAddress);
                SelectedUser = null!;
            }
            else
            {
                MessageBox.Show("Action cancelled.", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SearchPhrase = string.Empty;
        OnPropertyChanged(nameof(SearchPhrase));
        OnPropertyChanged(nameof(SelectedUser));
        OnPropertyChanged(nameof(Users));
        SelectedUser = null;
        UserFormViewModel.ShowUserForm = false;
    }

    private bool CanExecuteOnSelectedBorrower()
    {
        return HasSelectedUser;
    }

    private bool CanExecuteOnNewBorrower()
    {
        return !HasSelectedUser;
    }
}
