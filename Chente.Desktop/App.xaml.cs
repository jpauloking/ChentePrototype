using Chente.DataAccess;
using Chente.Desktop.ModelMappers;
using Chente.Desktop.Core;
using Chente.Desktop.Extensions.Configuration;
using Chente.Desktop.Services;
using Chente.Desktop.ViewModels;
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

    public App()
    {
        services.AddAutoMapper(typeof(BorrowerProfile), typeof(LoanProfile), typeof(InstallmentProfile));
        services.AddSingleton<ViewModelLocator>();
        services.AddSingleton<WindowMapper>();
        services.AddSingleton<WindowManager>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<ModalNavigationService>();
        services.AddSingleton<BorrowerStoreService>();
        services.AddSingleton<LoanStoreService>();
        services.AddSingleton<InstallmentStoreService>();
        services.AddSingleton(sp => ViewModelFactoryCreator(sp));
        services.AddDataAccess();
        services.AddViewModels();
        services.AddViews();
        serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        CreateDatabaseIfNotExist();
        ShowMainWindow();
        base.OnStartup(e);
    }

    private static Func<Type, ViewModelBase> ViewModelFactoryCreator(IServiceProvider sp)
    {
        return viewModelType => (ViewModelBase)sp.GetRequiredService(viewModelType);
    }

    private void ShowMainWindow()
    {
        MainViewModel mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
        WindowManager windowManager = serviceProvider.GetRequiredService<WindowManager>();
        windowManager.ShowWindow(mainViewModel);
    }

    private void CreateDatabaseIfNotExist()
    {
        ApplicationDbContextFactory dbContextFactory = serviceProvider.GetRequiredService<ApplicationDbContextFactory>();
        using ApplicationDbContext context = dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
    }
}

