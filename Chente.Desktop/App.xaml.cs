using Chente.DataAccess;
using Chente.Desktop.Extensions.Configuration;
using Chente.Desktop.Profiles;
using Chente.Desktop.ViewModels;
using Chente.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Chente.Desktop;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;
    private readonly IServiceCollection services = new ServiceCollection();
    // Todo - Use ApplicationHost to add logging, appsettings.json, Depedency injection, hosting

    public App()
    {
        services.AddAppServices();
        services.AddDataAccess();
        services.AddViewModels();
        services.AddViews();
        services.AddAutoMapper(typeof(BorrowerProfile), typeof(LoanProfile), typeof(InstallmentProfile));
        serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        CreateDatabaseIfNotExist();
        ShowMainWindow();
        base.OnStartup(e);
    }

    private void ShowMainWindow()
    {
        MainViewModel mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
        Window mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.DataContext = mainViewModel;
        mainWindow.Show();
    }

    private void CreateDatabaseIfNotExist()
    {
        ApplicationDbContextFactory dbContextFactory = serviceProvider.GetRequiredService<ApplicationDbContextFactory>();
        using ApplicationDbContext context = dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
    }
}

