using Chente.Desktop.Controls.Borrower.BorrowerList;
using Chente.Desktop.Controls.Dashboard.PeriodicSummary;
using Chente.Desktop.Controls.Installment.InstallmentList;
using Chente.Desktop.Controls.Loan.LoanList;
using Chente.Desktop.Core;
using Chente.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Extensions.Configuration;

public static class ConfigureViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<CashFlowViewModel>();
        services.AddSingleton<PeriodicSummaryViewModel>();
        services.AddSingleton<DatabaseSummaryViewModel>();
        services.AddSingleton<BorrowersViewModel>();
        services.AddTransient<BorrowerFormViewModel>();
        services.AddSingleton<BorrowerListViewModel>();
        services.AddSingleton<BorrowerDetailsViewModel>();
        services.AddSingleton<LoansViewModel>();
        services.AddTransient<LoanFormViewModel>();
        services.AddSingleton<LoanListViewModel>();
        services.AddSingleton<LoanDetailsViewModel>();
        services.AddSingleton<InstallmentsViewModel>();
        services.AddTransient<InstallmentFormViewModel>();
        services.AddSingleton<InstallmentListViewModel>();
        services.AddSingleton<InstallmentDetailsViewModel>();
        services.AddSingleton(sp => ViewModelCreator(sp));
        return services;
    }

    private static Func<Type, ViewModelBase> ViewModelCreator(IServiceProvider sp)
    {
        return viewModelType => (ViewModelBase)sp.GetRequiredService(viewModelType);
    }
}
