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
    private readonly WindowManager windowManager;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly IMapper mapper;

    public LoanFormViewModel(BorrowerStoreService borrowerStoreService, WindowManager windowManager, IMapper mapper)
    {
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
            MessageBox.Show("Task failed", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            var loan = new Domain.Models.Loan(DateOpened, Principal, InterestRate, DurationInDays, AmountPerInstallment);
            try
            {
                await borrowerStoreService.AddLoanToBorrower(loan);
                DateOpened = DateTime.Today;
                Principal = default;
                InterestRate = default;
                DurationInDays = default;
                AmountPerInstallment = default;
                windowManager.CloseModal<ModalViewModel>();
                MessageBox.Show("Task completed", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Domain.Exceptions.HasOutstandingLoanException)
            {
                MessageBox.Show($"Task failed. Please clear outstanding loans of borrower Number: {SelectedBorrower.BorrowerNumber} Name: {SelectedBorrower.DisplayName} before giving another loan.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
