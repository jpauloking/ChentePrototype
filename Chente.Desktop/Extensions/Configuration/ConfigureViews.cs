using Chente.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Extensions.Configuration;

public static class ConfigureViews
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        //services.AddSingleton(sp => new MainWindow
        //{
        //    DataContext = sp.GetRequiredService<MainViewModel>()
        //});
        services.AddSingleton<MainWindow>();
        services.AddSingleton<BorrowersView>();
        services.AddSingleton<LoansView>();
        services.AddSingleton<InstallmentsView>();
        return services;
    }
}
