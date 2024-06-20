using Chente.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Extensions;

public static class ConfigureViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<ModalViewModel>();
        services.AddSingleton<BorrowerViewModel>();
        services.AddSingleton<BorrowersViewModel>();
        services.AddSingleton<BorrowerFormViewModel>();
        services.AddSingleton<BorrowerListViewModel>();
        services.AddSingleton<BorrowerDetailsViewModel>();
        services.AddSingleton<LoansViewModel>();
        services.AddSingleton<LoanFormViewModel>();
        services.AddSingleton<LoanListViewModel>();
        services.AddSingleton<LoanDetailsViewModel>();
        services.AddSingleton<InstallmentsViewModel>();
        services.AddSingleton<InstallmentFormViewModel>();
        services.AddSingleton<InstallmentListViewModel>();
        services.AddSingleton<InstallmentDetailsViewModel>();
        return services;
    }
}
