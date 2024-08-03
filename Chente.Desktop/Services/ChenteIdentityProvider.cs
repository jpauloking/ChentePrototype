using System.Security.Claims;
using System.Security.Principal;

namespace Chente.Desktop.Services;

internal class ChenteIdentityProvider
{
    private static readonly char separator = System.IO.Path.DirectorySeparatorChar;
    public static string DomainName => GetCurrentUser().Name.Split(separator)[0];
    public static string Username => GetCurrentUser().Name.Split(separator)[1];
    public static bool IsLoggedIn => GetCurrentUser().IsAuthenticated;
    public static bool IsNotLoggedIn => GetCurrentUser().IsAnonymous;
    public static bool IsGuest => GetCurrentUser().IsGuest;
    public static bool IsSystem => GetCurrentUser().IsSystem; // Todo - Add authorization for super admin using IsSystem property
    private static WindowsIdentity GetCurrentUser() => WindowsIdentity.GetCurrent();

    public void AddClaim(string claimName, string claimValue)
    {
        // Todo - Use this method to add claims against which to perform authorization in the app
        Claim newClaim = new(claimName, claimValue, "ChenteClaim", "ChenteLoanPayments");
        GetCurrentUser().AddClaim(newClaim);
    }

    public bool HasClaim(string claimName, string claimValue)
    {
        // Todo - Use this method to check claims against which to perform authorization in the app
        return GetCurrentUser().HasClaim(claimName, claimValue);
    }
}
