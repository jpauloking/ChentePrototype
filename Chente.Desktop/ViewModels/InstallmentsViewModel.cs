using AutoMapper;
using Chente.Desktop.Controls.Installment.InstallmentList;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentsViewModel : ViewModelBase
{
    private readonly DateTime today = DateTime.Now;
    private readonly InstallmentListViewModel installmentListViewModel;
    private readonly InstallmentDetailsViewModel installmentDetailsViewModel;
    private readonly InstallmentFormViewModel installmentFormViewModel;
    private readonly LoanStoreService loanStoreService;
    private readonly InstallmentStoreService installmentStoreService;
    private readonly IMapper mapper;
    [ObservableProperty]
    private string pageTitle = "All installments";
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PayInstallmentCommand))]
    private bool hasSelectedInstallment = false;

    public DateTime DisplayDateStart => today.AddDays(-365);
    public DateTime DisplayDate => today;
    public DateTime DisplayDateEnd => today.AddDays(365);
    public InstallmentListViewModel InstallmentListViewModel => installmentListViewModel;
    public InstallmentDetailsViewModel InstallmentDetailsViewModel => installmentDetailsViewModel;
    public InstallmentFormViewModel InstallmentFormViewModel => installmentFormViewModel;
    public IEnumerable<LoanViewModel> Loans => mapper.Map<IEnumerable<LoanViewModel>>(loanStoreService.Loans);

    public string? SearchPhrase
    {
        get => installmentStoreService.SearchPhrase;
        set => installmentStoreService.SearchPhrase = value;
    }

    public LoanViewModel SelectedLoan
    {
        get => mapper.Map<LoanViewModel>(loanStoreService.SelectedLoan);
        set => loanStoreService.SelectedLoan = mapper.Map<Domain.Models.Loan>(value);
    }

    public DateTime? StartDate
    {
        get => installmentStoreService.StartDate;
        set => installmentStoreService.StartDate = value;
    }

    public DateTime? EndDate
    {
        get => installmentStoreService.EndDate;
        set => installmentStoreService.EndDate = value;
    }

    public bool IncludePaid
    {
        get => installmentStoreService.IncludePaid;
        set => installmentStoreService.IncludePaid = value;
    }

    public bool OnlyOverdue
    {
        get => installmentStoreService.OnlyOverdue;
        set => installmentStoreService.OnlyOverdue = value;
    }

    public InstallmentsViewModel(InstallmentListViewModel installmentListViewModel, InstallmentDetailsViewModel installmentDetailsViewModel, IMapper mapper, LoanStoreService loanStoreService, InstallmentStoreService installmentStoreService, InstallmentFormViewModel installmentFormViewModel)
    {
        this.installmentListViewModel = installmentListViewModel;
        this.installmentDetailsViewModel = installmentDetailsViewModel;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
        this.installmentStoreService = installmentStoreService;
        this.installmentStoreService.SelectedInstallmentChanged += OnSelectedInstallmentChanged;
        this.installmentFormViewModel = installmentFormViewModel;
    }

    private void OnSelectedInstallmentChanged(object? sender, EventArgs e)
    {
        HasSelectedInstallment = installmentStoreService.SelectedInstallment is not null;
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        PageTitle = SelectedLoan is not null ? $"{SelectedLoan.LoanNumber} installments" : "All installments";
        OnPropertyChanged(nameof(PageTitle));
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SearchPhrase = null!;
        StartDate = null!;
        EndDate = null!;
        IncludePaid = false;
        OnlyOverdue = false;
        SelectedLoan = null!;
        OnPropertyChanged(nameof(SearchPhrase));
        OnPropertyChanged(nameof(StartDate));
        OnPropertyChanged(nameof(EndDate));
        OnPropertyChanged(nameof(IncludePaid));
        OnPropertyChanged(nameof(OnlyOverdue));
        OnPropertyChanged(nameof(SelectedLoan));
        installmentFormViewModel.ShowInstallmentForm = false;
    }

    [RelayCommand(CanExecute = nameof(CanExecutePayInstallment))]
    private void PayInstallment()
    {
        if (installmentStoreService.SelectedInstallment is not null)
        {
            installmentFormViewModel.ShowInstallmentForm = true;
        }
        else
        {
            MessageBox.Show("Please select an installment and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    private bool CanExecutePayInstallment()
    {
        return HasSelectedInstallment;
    }
}
