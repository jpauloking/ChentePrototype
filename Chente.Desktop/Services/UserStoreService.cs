using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;
internal class UserStoreService
{
    private readonly UserRepository userRepository;
    private DataAccess.Models.User? selectedUser;
    private string? searchPhrase = null!;

    public ObservableCollection<DataAccess.Models.User> Users = [];
    public DataAccess.Models.User? SelectedUser
    {
        get => selectedUser;
        set
        {
            selectedUser = value;
            SelectedUserChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public string? SearchPhrase
    {
        get => searchPhrase;
        set
        {
            searchPhrase = value;
            GetAsync().GetAwaiter();
        }
    }

    public event EventHandler SelectedUserChanged = default!;
    public event EventHandler UsersCollectionChanged = default!;

    public UserStoreService(UserRepository userRepository)
    {
        this.userRepository = userRepository;
        GetAsync().GetAwaiter();
    }

    public async Task CreateAsync(DataAccess.Models.User user)
    {
        await userRepository.CreateAsync(user);
        await GetAsync();
    }

    public async Task DeleteAsync(string emailAddress)
    {
        var user = await GetAsync(emailAddress);
        if (user is not null)
        {
            await userRepository.DeleteAsync(user.Id);
        }
        await GetAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await userRepository.DeleteAsync(id);
        await GetAsync();
    }

    public async Task GetAsync()
    {
        var users = await userRepository.GetAsync();
        if (!string.IsNullOrEmpty(SearchPhrase))
        {
            users = users.Where(b => b.Username.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase) ||
            b.EmailAddress.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase) ||
            b.PhoneNumber!.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase));
        }
        if (Users.Any())
        {
            Users.Clear();
        }
        foreach (var borrower in users)
        {
            Users.Add(borrower);
        }
        SelectedUser = null;
        UsersCollectionChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task<DataAccess.Models.User> GetAsync(int id)
    {
        DataAccess.Models.User? user = await userRepository.GetAsync(id);
        return user!;
    }

    public async Task<DataAccess.Models.User> GetAsync(string emailAddress)
    {
        DataAccess.Models.User? user = await userRepository.GetAsync(emailAddress);
        return user!;
    }

    internal async Task UpdateAsync(DataAccess.Models.User user)
    {
        var userFromDatabase = await GetAsync(user.EmailAddress);
        if (userFromDatabase is not null)
        {
            await userRepository.UpdateAsync(user);
            await GetAsync();
        }
    }
}
