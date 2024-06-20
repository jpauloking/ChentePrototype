using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentsViewModel : ViewModelBase
{
    private readonly InstallmentListViewModel installmentListViewModel;
    private readonly InstallmentDetailsViewModel installmentDetailsViewModel;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;
    [ObservableProperty]
    private string pageTitle = "All installments";

    public InstallmentListViewModel InstallmentListViewModel => installmentListViewModel;
    public InstallmentDetailsViewModel InstallmentDetailsViewModel => installmentDetailsViewModel;
    public IEnumerable<BorrowerViewModel> Borrowers => mapper.Map<IEnumerable<BorrowerViewModel>>(borrowerStoreService.Borrowers);
    public IEnumerable<LoanViewModel> Loans => mapper.Map<IEnumerable<LoanViewModel>>(loanStoreService.Loans);
    public BorrowerViewModel SelectedBorrower
    {
        get => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);
        set => borrowerStoreService.SelectedBorrower = mapper.Map<Domain.Models.Borrower>(value);
    }
    public LoanViewModel SelectedLoan
    {
        get => mapper.Map<LoanViewModel>(loanStoreService.SelectedLoan);
        set => loanStoreService.SelectedLoan = mapper.Map<Domain.Models.Loan>(value);
    }

    public InstallmentsViewModel(InstallmentListViewModel installmentListViewModel, InstallmentDetailsViewModel installmentDetailsViewModel, BorrowerStoreService borrowerStoreService, IMapper mapper, LoanStoreService loanStoreService)
    {
        this.installmentListViewModel = installmentListViewModel;
        this.installmentDetailsViewModel = installmentDetailsViewModel;
        this.borrowerStoreService = borrowerStoreService;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        PageTitle = SelectedLoan is not null ? $"{SelectedLoan.LoanNumber} installments" : "All installments";
        OnPropertyChanged(nameof(PageTitle));
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        if (SelectedLoan is not null)
        {
            PageTitle = SelectedBorrower is not null ? $"{SelectedBorrower.DisplayName} {SelectedLoan.LoanNumber} installments" : "All installments";
        }
        else
        {
            PageTitle = SelectedBorrower is not null ? $"{SelectedBorrower.DisplayName}'s installments" : "All installments";
        }
        OnPropertyChanged(nameof(PageTitle));
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SelectedBorrower = null!;
        SelectedLoan = null!;
        OnPropertyChanged(nameof(SelectedBorrower));
        OnPropertyChanged(nameof(SelectedLoan));
    }
}
