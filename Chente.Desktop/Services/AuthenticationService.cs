
using Chente.Desktop.Exceptions;
using System.Security.Claims;
using System.Security.Principal;

namespace Chente.Desktop.Services;

internal class AuthenticationService
{
    private readonly UserStoreService userStoreService;
    public event EventHandler? AuthenticationStateChanged;

    public AuthenticationService(UserStoreService userStoreService)
    {
        this.userStoreService = userStoreService;
    }

    public async Task<IPrincipal?> LogIn(string emailAddress, string password)
    {
        DataAccess.Models.User user = await userStoreService.GetAsync(emailAddress);
        if (user is not null)
        {
            bool isPasswordCorrect = PasswordCorrect(password, user.Password);
            if (isPasswordCorrect)
            {
                ChenteIdentityProvider.AddClaim(ClaimTypes.Name, user.Username);
                ChenteIdentityProvider.AddClaim(ClaimTypes.Email, user.EmailAddress);
                ChenteIdentityProvider.AddClaim(ClaimTypes.MobilePhone, user.PhoneNumber!);
                ChenteIdentityProvider.AddClaim(ClaimTypes.Role, user.Role);
            }
            AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
            return ChenteIdentityProvider.Principal;
        }
        else
        {
            throw new AuthenticationFailedException("User was not found in the database. Check your credentials or Contact your system administrator.");
        }
    }

    private bool PasswordCorrect(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
