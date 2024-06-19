using Chente.Desktop.Core;
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

    public BorrowerListViewModel BorrowerListViewModel => borrowerListViewModel;
    public BorrowerDetailsViewModel BorrowerDetailsViewModel => borrowerDetailsViewModel;
    public string? SearchPhrase
    {
        get => borrowerStoreService.SearchPhrase;
        set => borrowerStoreService.SearchPhrase = value;
    }

    public BorrowersViewModel(ModalNavigationService modalNavigationService, WindowManager windowManager, BorrowerDetailsViewModel borrowerDetailsViewModel, BorrowerListViewModel borrowerListViewModel, BorrowerStoreService borrowerStoreService)
    {
        this.modalNavigationService = modalNavigationService;
        this.borrowerStoreService = borrowerStoreService;
        this.windowManager = windowManager;
        this.borrowerDetailsViewModel = borrowerDetailsViewModel;
        this.borrowerListViewModel = borrowerListViewModel;
    }

    [RelayCommand]
    private void AddNewBorrower()
    {
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
