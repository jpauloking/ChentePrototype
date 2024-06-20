using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentFormViewModel : ViewModelBase
{
    private static readonly DateTime today = DateTime.Today;
    private readonly WindowManager windowManager;
    private readonly InstallmentStoreService installmentStoreService;

    private DateTime dateDue = today;

    public Domain.Models.Installment? SelectedInstallment => installmentStoreService.SelectedInstallment;

    [DataType(DataType.Date)]
    public DateTime DateDue
    {
        get => dateDue;
        set => SetProperty(ref dateDue, value, true);
    }

    private decimal amount;

    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    public decimal Amount
    {
        get => amount;
        set => SetProperty(ref amount, value, true);
    }

    private decimal beginningBalance;

    [Range(0, Int16.MaxValue)]
    [Precision(16, 4)]
    public decimal BeginningBalance
    {
        get => beginningBalance;
        set => SetProperty(ref beginningBalance, value, true);
    }

    private decimal endingBalance;

    [Range(0, Int16.MaxValue)]
    [Precision(16, 4)]
    public decimal EndingBalance
    {
        get => endingBalance;
        set => SetProperty(ref endingBalance, value, true);
    }

    private decimal amountPaid;

    [Range(0, Int16.MaxValue)]
    [Precision(16, 4)]
    public decimal AmountPaid
    {
        get => amountPaid;
        set => SetProperty(ref amountPaid, value, true);
    }

    private DateTime datePaid = today;

    [DataType(DataType.Date)]
    public DateTime DatePaid
    {
        get => datePaid;
        set => SetProperty(ref datePaid, value, true);
    }

    public InstallmentFormViewModel(WindowManager windowManager, InstallmentStoreService installmentStoreService)
    {
        this.windowManager = windowManager;
        this.installmentStoreService = installmentStoreService;
        if (SelectedInstallment is not null)
        {
            DateDue = SelectedInstallment.DateDue;
            Amount = Math.Round(SelectedInstallment.Amount);
            BeginningBalance = Math.Round(SelectedInstallment.BeginningBalance);
            EndingBalance = Math.Round(SelectedInstallment.EndingBalance);
            AmountPaid = SelectedInstallment.AmountPaid == 0 ? Math.Round(SelectedInstallment.Amount) : Math.Round(SelectedInstallment.AmountPaid);
        }
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
                await installmentStoreService.PayInstallmentAsync(selectedInstallment, Amount, DatePaid);
                windowManager.CloseModal<ModalViewModel>();
                MessageBox.Show("Task completed", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
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
        windowManager.CloseModal<ModalViewModel>();
    }

}
