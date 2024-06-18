using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
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

    //[ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(BorrowerDetailsViewModel))]
    //private BorrowerViewModel? selectedBorrower;

    //public IEnumerable<BorrowerViewModel> Borrowers => mapper.Map<IEnumerable<BorrowerViewModel>>(loanStoreService.Borrowers);
    //public NavigationService NavigationService => navigationService;
    public BorrowerListViewModel BorrowerListViewModel => borrowerListViewModel;
    public BorrowerDetailsViewModel BorrowerDetailsViewModel => borrowerDetailsViewModel;

    public BorrowersViewModel(ModalNavigationService modalNavigationService, WindowManager windowManager, BorrowerDetailsViewModel borrowerDetailsViewModel, BorrowerListViewModel borrowerListViewModel, IMapper mapper, BorrowerStoreService borrowerStoreService)
    {
        this.modalNavigationService = modalNavigationService;
        this.borrowerStoreService = borrowerStoreService;
        this.windowManager = windowManager;
        this.borrowerDetailsViewModel = borrowerDetailsViewModel;
        this.borrowerListViewModel = borrowerListViewModel;
        this.mapper = mapper;
    }

    [RelayCommand]
    private void AddNewBorrower()
    {
        modalNavigationService.NavigateTo<BorrowerFormViewModel>();
        windowManager.ShowModal<ModalViewModel>();
    }
}
