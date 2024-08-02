using Chente.Desktop.Controls.Borrower.BorrowerList;
using Chente.Desktop.Controls.Installment.InstallmentList;
using Chente.Desktop.Controls.Loan.LoanList;
using Chente.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Extensions.Configuration;

public static class ConfigureViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<ModalViewModel>();
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
        return services;
    }
}
