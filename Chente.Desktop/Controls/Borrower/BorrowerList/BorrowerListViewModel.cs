using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using Chente.Desktop.ViewModels;

namespace Chente.Desktop.Controls.Borrower.BorrowerList;

internal partial class BorrowerListViewModel : ViewModelBase
{
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly IMapper mapper;

    public bool HasBorrowers => Borrowers.Any();
    public bool HasNoBorrowers => !HasBorrowers;
    public IEnumerable<BorrowerViewModel> Borrowers => mapper.Map<IEnumerable<BorrowerViewModel>>(borrowerStoreService.Borrowers);
    public BorrowerViewModel SelectedBorrower
    {
        get => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);
        set => borrowerStoreService.SelectedBorrower = mapper.Map<Domain.Models.Borrower>(value);
    }

    public BorrowerListViewModel(BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
        this.borrowerStoreService.BorrowersCollectionChanged += OnBorrowerAdded;
    }

    private void OnBorrowerAdded(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Borrowers));
        OnPropertyChanged(nameof(HasBorrowers));
        OnPropertyChanged(nameof(HasNoBorrowers));
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower borrower)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
    }
}
