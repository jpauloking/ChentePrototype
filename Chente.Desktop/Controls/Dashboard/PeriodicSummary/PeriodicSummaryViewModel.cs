using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using Chente.Desktop.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.Controls.Dashboard.PeriodicSummary;

internal partial class PeriodicSummaryViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly DataSummarizationService dataSummarizationService;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InstallmentsDueWithinPeriod))]
    [NotifyPropertyChangedFor(nameof(UpcomingInstallmentsCount))]
    [NotifyPropertyChangedFor(nameof(UpcomingInstallmentsAmount))]
    [NotifyPropertyChangedFor(nameof(OverDueInstallmentsCount))]
    [NotifyPropertyChangedFor(nameof(OverDueInstallmentsAmountDue))]
    [NotifyPropertyChangedFor(nameof(PeriodCategorySummary))]
    private int selectedInstallmentsDueWithinPeriodDaysAheadCount = 14;

    public List<int> InstallmentsDueWithinPeriodDaysAheadCounts => [7, 14, 21, 28];

    public IEnumerable<InstallmentViewModel> InstallmentsDueWithinPeriod => mapper.Map<IEnumerable<InstallmentViewModel>>(dataSummarizationService.Installments.Where(i => !i.IsPaid && ((i.IsOverDue || i.DateDue == DateTime.Today || i.DateDue <= (DateTime.Today).AddDays(SelectedInstallmentsDueWithinPeriodDaysAheadCount)))));

    public int UpcomingInstallmentsCount => InstallmentsDueWithinPeriod.Count();
    public decimal UpcomingInstallmentsAmount => InstallmentsDueWithinPeriod.Sum(i => i.Amount);
    public decimal UpcomingInstallmentsAmountDue => InstallmentsDueWithinPeriod.Sum(i => i.AmountDue);
    public int OverDueInstallmentsCount => InstallmentsDueWithinPeriod.Where(i => i.IsOverDue).Count();
    public decimal OverDueInstallmentsAmountDue => InstallmentsDueWithinPeriod.Where(i => i.IsOverDue).Sum(i => i.AmountDue);

    public List<CategorySummaryItem> PeriodCategorySummary => [
        new()
        {
            Category = "Upcoming installments",
            Number = UpcomingInstallmentsCount,
            Amount = UpcomingInstallmentsAmount,
        },
        new()
        {
            Category = "Overdue installments",
            Number = OverDueInstallmentsCount,
            Amount = OverDueInstallmentsAmountDue,
        },
    ];

    public PeriodicSummaryViewModel(IMapper mapper, DataSummarizationService dataSummarizationService)
    {
        this.mapper = mapper;
        this.dataSummarizationService = dataSummarizationService;
    }
}
