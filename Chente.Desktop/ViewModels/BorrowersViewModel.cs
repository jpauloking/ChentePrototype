using AutoMapper;
using Chente.Desktop.Controls.Borrower.BorrowerList;
using Chente.Desktop.Core;
using Chente.Desktop.Extensions.ModelExtensions;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.Input;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowersViewModel : ViewModelBase
{
    private readonly ModalNavigationService modalNavigationService;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly WindowManager windowManager;
    private readonly BorrowerListViewModel borrowerListViewModel;
    private readonly BorrowerDetailsViewModel borrowerDetailsViewModel;
    private readonly IMapper mapper;

    public BorrowerListViewModel BorrowerListViewModel => borrowerListViewModel;
    public BorrowerDetailsViewModel BorrowerDetailsViewModel => borrowerDetailsViewModel;
    public IEnumerable<BorrowerViewModel> Borrowers => borrowerStoreService.Borrowers.Select(b => b.MapToBorrowerViewModel(mapper));
    public string? SearchPhrase
    {
        get => borrowerStoreService.SearchPhrase;
        set => borrowerStoreService.SearchPhrase = value;
    }
    public BorrowerViewModel? SelectedBorrower
    {
        get => borrowerStoreService.SelectedBorrower!.MapToBorrowerViewModel(mapper);
        set => borrowerStoreService.SelectedBorrower = value!.MapToDomainBorrower(mapper);
    }

    public BorrowersViewModel(ModalNavigationService modalNavigationService, WindowManager windowManager, BorrowerDetailsViewModel borrowerDetailsViewModel, BorrowerListViewModel borrowerListViewModel, BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.modalNavigationService = modalNavigationService;
        this.borrowerStoreService = borrowerStoreService;
        this.windowManager = windowManager;
        this.borrowerDetailsViewModel = borrowerDetailsViewModel;
        this.borrowerListViewModel = borrowerListViewModel;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
    }

    [RelayCommand]
    private void AddNewBorrower()
    {
        if (borrowerStoreService.SelectedBorrower is not null)
        {
            borrowerStoreService.SelectedBorrower = null;
        }
        modalNavigationService.NavigateTo<BorrowerFormViewModel>();
        windowManager.ShowModal<ModalViewModel>();
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SearchPhrase = string.Empty;
        OnPropertyChanged(nameof(SearchPhrase));
    }
}
