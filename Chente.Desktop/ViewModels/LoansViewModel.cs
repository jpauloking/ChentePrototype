using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.Input;

namespace Chente.Desktop.ViewModels;

internal partial class LoansViewModel : ViewModelBase
{
    private readonly ModalNavigationService modalNavigationService;
    private readonly WindowManager windowManager;
    private readonly LoanListViewModel loanListViewModel;
    private readonly LoanDetailsViewModel loanDetailsViewModel;

    public LoanListViewModel LoanListViewModel => loanListViewModel;
    public LoanDetailsViewModel LoanDetailsViewModel => loanDetailsViewModel;

    public LoansViewModel(ModalNavigationService modalNavigationService, WindowManager windowManager, LoanDetailsViewModel loanDetailsViewModel, LoanListViewModel loanListViewModel)
    {
        this.modalNavigationService = modalNavigationService;
        this.windowManager = windowManager;
        this.loanDetailsViewModel = loanDetailsViewModel;
        this.loanListViewModel = loanListViewModel;
    }

    [RelayCommand]
    private void AddNewLoan()
    {
        modalNavigationService.NavigateTo<LoanFormViewModel>();
        windowManager.ShowModal<ModalViewModel>();
    }
}
