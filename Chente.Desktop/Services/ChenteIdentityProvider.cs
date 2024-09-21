using System.Security.Claims;
using System.Security.Principal;

namespace Chente.Desktop.Services;

internal class ChenteIdentityProvider
{
    public static event EventHandler? IdentityChanged;

    public static void AddClaim(string claimName, string claimValue)
    {
        GenericPrincipal? principal = (GenericPrincipal?)Thread.CurrentPrincipal;
        if (principal?.Identity.AuthenticationType != "Chente")
        {
            var identity = new GenericIdentity("Chente", "Chente");
            principal = new(identity, []);
            Claim newClaim = new(claimName, claimValue, null, "Chente");
            ((GenericIdentity)principal.Identity).AddClaim(newClaim);
            Thread.CurrentPrincipal = principal;
        }
        else
        {
            Claim newClaim = new(claimName, claimValue, null, "Chente");
            if (principal.Identity is null)
            {
                var identity = new GenericIdentity("Chente", "Chente");
                identity.AddClaim(newClaim);
                principal.AddIdentity(identity);
            }
            else
            {
                var identity = (GenericIdentity)principal.Identity;
                identity.AddClaim(newClaim);
            }
        }
        IdentityChanged?.Invoke(null, EventArgs.Empty);
    }

    public static bool HasClaim(string claimName)
    {
        GenericPrincipal? principal = (GenericPrincipal?)Thread.CurrentPrincipal;
        if (principal is null)
        {
            return false;
        }
        if (!principal.Identity.IsAuthenticated)
        {
            return false;
        }
        if (principal.Identity.Name != "Chente")
        {
            return false;
        }
        return principal.IsInRole(claimName);
    }

}
