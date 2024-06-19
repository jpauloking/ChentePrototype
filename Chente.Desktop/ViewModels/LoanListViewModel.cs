using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal class LoanListViewModel : ViewModelBase
{
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;

    public bool HasLoans => Loans.Any();
    public bool HasNoLoans => !HasLoans;
    public IEnumerable<LoanViewModel> Loans => mapper.Map<IEnumerable<LoanViewModel>>(loanStoreService.Loans);
    public LoanViewModel SelectedLoan
    {
        get => mapper.Map<LoanViewModel>(loanStoreService.SelectedLoan);
        set => loanStoreService.SelectedLoan = mapper.Map<Domain.Models.Loan>(value);
    }

    public LoanListViewModel(LoanStoreService loanStoreService, IMapper mapper)
    {
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.loanStoreService.LoansCollectionChanged += OnLoansCollectionChanged;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        OnPropertyChanged(nameof(SelectedLoan));
    }

    private void OnLoansCollectionChanged(object? sender, EventArgs e)
    {
        SelectedLoan = null!;
        OnPropertyChanged(nameof(Loans));
        OnPropertyChanged(nameof(HasLoans));
        OnPropertyChanged(nameof(HasNoLoans));
    }
}
