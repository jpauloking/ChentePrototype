using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentListViewModel : ViewModelBase
{
    private readonly InstallmentStoreService installmentStoreService;
    private IMapper mapper;

    public bool HasInstallments => Installments.Any();
    public bool HasNoInstallments => !HasInstallments;

    public IEnumerable<InstallmentViewModel> Installments => mapper.Map<IEnumerable<InstallmentViewModel>>(installmentStoreService.Installments);
    public InstallmentViewModel SelectedInstallment
    {
        get => mapper.Map<InstallmentViewModel>(installmentStoreService.SelectedInstallment);
        set => installmentStoreService.SelectedInstallment = mapper.Map<Domain.Models.Installment>(value);
    }

    public InstallmentListViewModel(InstallmentStoreService installmentStoreService, IMapper mapper)
    {
        this.installmentStoreService = installmentStoreService;
        this.mapper = mapper;
        this.installmentStoreService.SelectedInstallmentChanged += OnInstallmentChanged;
        this.installmentStoreService.InstallmentsCollectionChanged += OnInstallmentsCollectionChanged;
    }

    private void OnInstallmentsCollectionChanged(object? sender, EventArgs e)
    {
        SelectedInstallment = null!;
        OnPropertyChanged(nameof(Installments));
        OnPropertyChanged(nameof(HasInstallments));
        OnPropertyChanged(nameof(HasNoInstallments));
    }

    private void OnInstallmentChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(SelectedInstallment));
    }
}
