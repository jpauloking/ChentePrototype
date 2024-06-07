using Chente.DataAccess.Repositories;
using Chente.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Extensions;

internal static class ConfigureDataAccessLayer
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<ApplicationDbContextFactory>();
        services.AddSingleton<BorrowerRepository>();
        services.AddSingleton<LoanRepository>();
        services.AddSingleton<InstallmentRepository>();
        return services;
    }
}
