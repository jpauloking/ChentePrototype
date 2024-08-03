using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentFormViewModel : ViewModelBase
{
    private static readonly DateTime today = DateTime.Today;
    private readonly InstallmentStoreService installmentStoreService;
    private readonly LoanStoreService loanStoreService;
    [ObservableProperty]
    private bool showInstallmentForm;

    public DateTime DisplayDateStart => today.AddDays(-365);
    public DateTime DisplayDateEnd => today.AddDays(365);
    public Domain.Models.Installment? SelectedInstallment => installmentStoreService.SelectedInstallment;
    public string? SelectedLoan => loanStoreService.SelectedLoan?.LoanNumber;

    [DataType(DataType.Date)]
    [ObservableProperty]
    private DateTime dateDue = today;

    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    [ObservableProperty]
    private decimal amount;

    [Precision(16, 4)]
    [ObservableProperty]
    private decimal beginningBalance;

    [Precision(16, 4)]
    [ObservableProperty]
    private decimal endingBalance;

    [Precision(16, 4)]
    [ObservableProperty]
    private decimal amountDue;

    [Precision(16, 4)]
    [ObservableProperty]
    private decimal amountPaid;

    [Precision(16, 4)]
    [ObservableProperty]
    private decimal amountToPay;

    [DataType(DataType.Date)]
    [ObservableProperty]
    private DateTime datePaid = today;

    public InstallmentFormViewModel(InstallmentStoreService installmentStoreService, LoanStoreService loanStoreService)
    {
        this.installmentStoreService = installmentStoreService;
        this.loanStoreService = loanStoreService;
        this.installmentStoreService.SelectedInstallmentChanged += OnSelectedInstallmentChanged;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
        if (SelectedInstallment is not null)
        {
            DateDue = SelectedInstallment.DateDue;
            Amount = Math.Round(SelectedInstallment.Amount);
            BeginningBalance = Math.Round(SelectedInstallment.BeginningBalance);
            EndingBalance = Math.Round(SelectedInstallment.EndingBalance);
            AmountPaid = Math.Round(SelectedInstallment.AmountPaid);
            // Amount due is used to populate the text box with the amount to be paid and is passed to the PayInstallment function in the model.
            AmountDue = Math.Round(SelectedInstallment.AmountDue);
            AmountToPay = AmountDue;
        }
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        if (SelectedInstallment is not null)
        {
            DateDue = SelectedInstallment.DateDue;
            Amount = Math.Round(SelectedInstallment.Amount);
            BeginningBalance = Math.Round(SelectedInstallment.BeginningBalance);
            EndingBalance = Math.Round(SelectedInstallment.EndingBalance);
            AmountPaid = Math.Round(SelectedInstallment.AmountPaid);
            AmountDue = Math.Round(SelectedInstallment.AmountDue);
            AmountToPay = AmountDue;
            DatePaid = SelectedInstallment.DatePaid;
        }
        OnPropertyChanged(nameof(SelectedLoan));
        OnPropertyChanged(nameof(SelectedInstallment));
    }

    private void OnSelectedInstallmentChanged(object? sender, EventArgs e)
    {
        if (SelectedInstallment is not null)
        {
            DateDue = SelectedInstallment.DateDue;
            Amount = Math.Round(SelectedInstallment.Amount);
            BeginningBalance = Math.Round(SelectedInstallment.BeginningBalance);
            EndingBalance = Math.Round(SelectedInstallment.EndingBalance);
            AmountPaid = Math.Round(SelectedInstallment.AmountPaid);
            AmountDue = Math.Round(SelectedInstallment.AmountDue);
            AmountToPay = AmountDue;
            DatePaid = SelectedInstallment.DatePaid;
        }
        OnPropertyChanged(nameof(SelectedInstallment));
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
            // Todo - Add null check to handle cases where installmentStoreService.SelectedInstallment is null
            //var installment = new Domain.Models.Installment(DateDue, Amount, BeginningBalance, EndingBalance, AmountPaid);
            Domain.Models.Installment selectedInstallment = installmentStoreService.SelectedInstallment!;
            try
            {
                // Todo - Consider case where Amount != PaymentBeingPaid. In which case installment.Amount != paymentAmount
                await installmentStoreService.PayInstallmentAsync(selectedInstallment, AmountToPay, DatePaid);
                MessageBox.Show("Task completed", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowInstallmentForm = false;
            }
            catch (Domain.Exceptions.InvalidPaymentException)
            {
                MessageBox.Show($"Task failed. Installment Number: {installmentStoreService.SelectedInstallment!.InstallmentNumber} has already been paid. Please select an unpaid installment and try again.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        ShowInstallmentForm = false;
    }

}
