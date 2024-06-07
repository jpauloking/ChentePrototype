using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
namespace Chente.Desktop.ViewModels;

internal partial class BorrowerListViewModel : ViewModelBase
{
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly IMapper mapper;

    public IEnumerable<BorrowerViewModel> Borrowers => mapper.Map<IEnumerable<BorrowerViewModel>>(borrowerStoreService.Borrowers);
    public BorrowerViewModel SelectedBorrower
    {
        get => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);
        set
        {
            borrowerStoreService.SelectedBorrower = mapper.Map<Domain.Models.Borrower>(value);
        }
    }

    public BorrowerListViewModel(BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower borrower)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
    }
}
