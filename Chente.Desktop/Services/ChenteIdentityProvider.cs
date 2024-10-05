using System.Security.Claims;
using System.Security.Principal;

namespace Chente.Desktop.Services;

internal class ChenteIdentityProvider
{
    public static IPrincipal? Principal { get; private set; }

    public static void AddClaim(string claimName, string claimValue)
    {
        Principal = (GenericPrincipal?)Thread.CurrentPrincipal;
        if (Principal?.Identity?.AuthenticationType != "Chente")
        {
            var identity = new GenericIdentity("Chente", "Chente");
            Principal = new GenericPrincipal(identity, []);
            Claim newClaim = new(claimName, claimValue, null, "Chente");
            ((GenericIdentity)Principal.Identity).AddClaim(newClaim);
        }
        else
        {
            Claim newClaim = new(claimName, claimValue, null, "Chente");
            if (Principal.Identity is null)
            {
                var identity = new GenericIdentity("Chente", "Chente");
                identity.AddClaim(newClaim);
                ((GenericPrincipal)Principal).AddIdentity(identity);
            }
            else
            {
                var identity = (GenericIdentity)Principal.Identity;
                identity.AddClaim(newClaim);
            }
        }
        Thread.CurrentPrincipal = Principal;
    }

    public static bool HasClaim(string claimName)
    {
        if (Principal is null)
        {
            return false;
        }
        if (Principal.Identity is null)
        {
            return false;
        }
        if (!Principal.Identity.IsAuthenticated)
        {
            return false;
        }
        if (Principal.Identity.Name != "Chente")
        {
            return false;
        }
        return Principal.IsInRole(claimName);
    }

}
