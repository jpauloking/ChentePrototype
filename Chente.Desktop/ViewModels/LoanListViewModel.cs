using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal class LoanListViewModel : ViewModelBase
{
    private readonly LoanStoreService loanStoreService;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly IMapper mapper;

    public IEnumerable<LoanViewModel> Loans => mapper.Map<IEnumerable<LoanViewModel>>(loanStoreService.Loans);
    public BorrowerViewModel SelectedBorrower => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

    public LoanListViewModel(LoanStoreService loanStoreService, BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.loanStoreService = loanStoreService;
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;

        borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
    }
}
