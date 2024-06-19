using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class LoansViewModel : ViewModelBase
{
    private readonly DateTime today = DateTime.Now;
    private readonly IMapper mapper;
    private readonly ModalNavigationService modalNavigationService;
    private readonly WindowManager windowManager;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly LoanStoreService loanStoreService;
    private readonly LoanListViewModel loanListViewModel;
    private readonly LoanDetailsViewModel loanDetailsViewModel;
    [ObservableProperty]
    private string pageTitle = "All loans";

    public DateTime DisplayDateStart => today.AddDays(-30);
    public DateTime DisplayDate => today;
    public DateTime DisplayDateEnd => today.AddDays(30);
    public LoanListViewModel LoanListViewModel => loanListViewModel;
    public LoanDetailsViewModel LoanDetailsViewModel => loanDetailsViewModel;
    public IEnumerable<BorrowerViewModel> Borrowers => mapper.Map<IEnumerable<BorrowerViewModel>>(borrowerStoreService.Borrowers);
    public BorrowerViewModel SelectedBorrower
    {
        get => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);
        set => borrowerStoreService.SelectedBorrower = mapper.Map<Domain.Models.Borrower>(value);
    }
    public DateTime? StartDate
    {
        get => loanStoreService.StartDate; 
        set => loanStoreService.StartDate = value;
    }
    public DateTime? EndDate
    {
        get => loanStoreService.EndDate;
        set => loanStoreService.EndDate = value;
    }

    public LoansViewModel(IMapper mapper, ModalNavigationService modalNavigationService, WindowManager windowManager, BorrowerStoreService borrowerStoreService, LoanDetailsViewModel loanDetailsViewModel, LoanListViewModel loanListViewModel, LoanStoreService loanStoreService)
    {
        this.mapper = mapper;
        this.modalNavigationService = modalNavigationService;
        this.windowManager = windowManager;
        this.borrowerStoreService = borrowerStoreService;
        this.loanDetailsViewModel = loanDetailsViewModel;
        this.loanListViewModel = loanListViewModel;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;

        PageTitle = borrowerStoreService.SelectedBorrower is not null ? $"{SelectedBorrower?.DisplayName}'s loans" : "All loans";
        this.loanStoreService = loanStoreService;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        PageTitle = SelectedBorrower is not null ? $"{SelectedBorrower.DisplayName}'s loans" : "All loans";
        OnPropertyChanged(nameof(PageTitle));
    }

    [RelayCommand]
    private void AddNewLoan()
    {
        if (SelectedBorrower is not null)
        {
            modalNavigationService.NavigateTo<LoanFormViewModel>();
            windowManager.ShowModal<ModalViewModel>();
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        StartDate = null!;
        EndDate = null!;
        SelectedBorrower = null!;
        OnPropertyChanged(nameof(StartDate));
        OnPropertyChanged(nameof(EndDate));
        OnPropertyChanged(nameof(SelectedBorrower));
    }
}
