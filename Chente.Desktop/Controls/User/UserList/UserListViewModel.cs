using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal partial class UserListViewModel : ViewModelBase
{
    private readonly UserStoreService userStoreService;
    private readonly IMapper mapper;

    public bool HasUsers => Users.Any();
    public bool HasNoUsers => !HasUsers;
    public IEnumerable<UserViewModel> Users => mapper.Map<IEnumerable<UserViewModel>>(userStoreService.Users);
    public UserViewModel SelectedUser
    {
        get => mapper.Map<UserViewModel>(userStoreService.SelectedUser);
        set => userStoreService.SelectedUser = mapper.Map<DataAccess.Models.User>(value);
    }

    public UserListViewModel(UserStoreService userStoreService, IMapper mapper)
    {
        this.mapper = mapper;
        this.userStoreService = userStoreService;
        this.userStoreService.SelectedUserChanged += OnSelectedUserChanged;
        this.userStoreService.UsersCollectionChanged += OnUsersCollectionChanged;
    }

    private void OnUsersCollectionChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Users));
        OnPropertyChanged(nameof(HasUsers));
        OnPropertyChanged(nameof(HasNoUsers));
        OnPropertyChanged(nameof(SelectedUser));
    }

    private void OnSelectedUserChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Users));
        OnPropertyChanged(nameof(HasUsers));
        OnPropertyChanged(nameof(HasNoUsers));
        OnPropertyChanged(nameof(SelectedUser));
    }
}
