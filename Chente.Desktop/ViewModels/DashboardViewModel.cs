using Chente.Desktop.Controls.Dashboard.PeriodicSummary;
using Chente.Desktop.Core;

namespace Chente.Desktop.ViewModels;

partial class DashboardViewModel : ViewModelBase
{
    private readonly CashFlowViewModel cashFlowViewModel;
    private readonly PeriodicSummaryViewModel periodicSummaryViewModel;
    private readonly DatabaseSummaryViewModel databaseSummaryViewModel;

    public CashFlowViewModel CashFlowViewModel => cashFlowViewModel;
    public PeriodicSummaryViewModel PeriodicSummaryViewModel => periodicSummaryViewModel;
    public DatabaseSummaryViewModel DatabaseSummaryViewModel => databaseSummaryViewModel;

    public DashboardViewModel(CashFlowViewModel cashFlowViewModel, PeriodicSummaryViewModel periodicSummaryViewModel, DatabaseSummaryViewModel databaseSummaryViewModel)
    {
        this.cashFlowViewModel = cashFlowViewModel;
        this.periodicSummaryViewModel = periodicSummaryViewModel;
        this.databaseSummaryViewModel = databaseSummaryViewModel;
    }
}
