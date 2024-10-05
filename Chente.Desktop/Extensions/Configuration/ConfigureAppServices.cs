using Chente.Desktop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Extensions.Configuration;

internal static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<NavigationService>();
        services.AddSingleton<BorrowerStoreService>();
        services.AddSingleton<LoanStoreService>();
        services.AddSingleton<InstallmentStoreService>();
        services.AddSingleton<UserStoreService>();
        services.AddSingleton<DataSummarizationService>();
        services.AddSingleton<AuthenticationService>();
        return services;
    }
}