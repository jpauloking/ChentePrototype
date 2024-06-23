using AutoMapper;
using Chente.Desktop.Controls.Installment.InstallmentList;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentsViewModel : ViewModelBase
{
    private readonly InstallmentListViewModel installmentListViewModel;
    private readonly InstallmentDetailsViewModel installmentDetailsViewModel;
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;
    [ObservableProperty]
    private string pageTitle = "All installments";

    public InstallmentListViewModel InstallmentListViewModel => installmentListViewModel;
    public InstallmentDetailsViewModel InstallmentDetailsViewModel => installmentDetailsViewModel;
    public IEnumerable<LoanViewModel> Loans => mapper.Map<IEnumerable<LoanViewModel>>(loanStoreService.Loans);
    public LoanViewModel SelectedLoan
    {
        get => mapper.Map<LoanViewModel>(loanStoreService.SelectedLoan);
        set => loanStoreService.SelectedLoan = mapper.Map<Domain.Models.Loan>(value);
    }

    public InstallmentsViewModel(InstallmentListViewModel installmentListViewModel, InstallmentDetailsViewModel installmentDetailsViewModel, IMapper mapper, LoanStoreService loanStoreService)
    {
        this.installmentListViewModel = installmentListViewModel;
        this.installmentDetailsViewModel = installmentDetailsViewModel;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        PageTitle = SelectedLoan is not null ? $"{SelectedLoan.LoanNumber} installments" : "All installments";
        OnPropertyChanged(nameof(PageTitle));
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SelectedLoan = null!;
        OnPropertyChanged(nameof(SelectedLoan));
    }
}
