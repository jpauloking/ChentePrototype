﻿using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class LoanFormViewModel : ViewModelBase
{
    private static readonly DateTime today = DateTime.Today;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;
    [ObservableProperty]
    private bool showLoanForm;

    public LoanFormViewModel(BorrowerStoreService borrowerStoreService, LoanStoreService loanStoreService, IMapper mapper)
    {
        this.borrowerStoreService = borrowerStoreService;
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    public DateTime DisplayDateStart => today.AddDays(-365);
    public DateTime DisplayDateEnd => today.AddDays(365);
    public BorrowerViewModel SelectedBorrower => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

    [DataType(DataType.Date)]
    [ObservableProperty]
    private DateTime dateOpened = today;

    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    [ObservableProperty]
    private decimal principal;

    [Range(0, Int16.MaxValue)]
    [ObservableProperty]
    private double interestRate;

    [Range(0, Int16.MaxValue)]
    [ObservableProperty]
    private int durationInDays;

    [Precision(16, 4)]
    [ObservableProperty]
    private decimal amountPerInstallment;

    [RelayCommand]
    private async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var loan = new Domain.Models.Loan(DateOpened, Principal, InterestRate, DurationInDays, AmountPerInstallment);
            try
            {
                await loanStoreService.AddLoan(loan);
                DateOpened = DateTime.Today;
                Principal = default;
                InterestRate = default;
                DurationInDays = default;
                AmountPerInstallment = default;
                ShowLoanForm = false;
            }
            catch (Domain.Exceptions.HasOutstandingLoanException)
            {
                MessageBox.Show($"Task failed. Please clear outstanding loans of borrower Number: {SelectedBorrower.BorrowerNumber} Name: {SelectedBorrower.DisplayName} before giving another loan.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show($"Task failed. Something went wrong while CHENTE LOAN PAYMENTS was trying to save loan.", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Task failed", "System says", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        ShowLoanForm = false;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower borrower)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
    }

}
