using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using AutoMapper;

namespace Chente.Desktop.ViewModels;

internal partial class LoanFormViewModel : ViewModelBase
{
    private static readonly DateTime today = DateTime.Today;
    private readonly LoanStoreService loanStoreService;
    private readonly WindowManager windowManager;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly IMapper mapper;

    public LoanFormViewModel(LoanStoreService loanStoreService, BorrowerStoreService borrowerStoreService, WindowManager windowManager, IMapper mapper)
    {
        this.loanStoreService = loanStoreService;
        this.windowManager = windowManager;
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    public DateTime DisplayDateStart => today.AddDays(-30);
    public DateTime DisplayDateEnd => today.AddDays(30);
    public BorrowerViewModel SelectedBorrower => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

    private DateTime dateOpened = today;

    [DataType(DataType.Date)]
    public DateTime DateOpened
    {
        get => dateOpened;
        set => SetProperty(ref dateOpened, value, true);
    }

    private decimal principal;

    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    public decimal Principal
    {
        get => principal;
        set => SetProperty(ref principal, value, true);
    }

    private double interestRate;

    [Range(0, Int16.MaxValue)]
    public double InterestRate
    {
        get => interestRate;
        set => SetProperty(ref interestRate, value, true);
    }

    private int durationInDays;

    [Range(0, Int16.MaxValue)]
    public int DurationInDays
    {
        get => durationInDays;
        set => SetProperty(ref durationInDays, value, true);
    }

    private decimal amountPerInstallment;

    [Range(0, Int16.MaxValue)]
    [Precision(16, 4)]
    public decimal AmountPerInstallment
    {
        get => amountPerInstallment;
        set => SetProperty(ref amountPerInstallment, value, true);
    }

    [RelayCommand]
    private async Task Save()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            MessageBox.Show("Task failed", "System says");
        }
        else
        {
            DataAccess.Models.Borrower selectedBorrower = mapper.Map<DataAccess.Models.Borrower>(SelectedBorrower);
            selectedBorrower.Id = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(SelectedBorrower.BorrowerNumber);
            var loan = new DataAccess.Models.Loan
            {
                Borrower = selectedBorrower,
                DateOpened = dateOpened,
                Principal = principal,
                InterestRate = interestRate,
                DurationInDays = durationInDays,
                AmountPerInstallment = amountPerInstallment
            };
            await borrowerStoreService.AddLoanToBorrower(loan);
            //await loanStoreService.CreateAsync(loan);
            MessageBox.Show("Task completed", "System says");
            windowManager.CloseModal<ModalViewModel>();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        windowManager.CloseModal<ModalViewModel>();
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower borrower)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
    }

}
