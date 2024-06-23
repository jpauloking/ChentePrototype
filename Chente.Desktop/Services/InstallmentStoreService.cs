using AutoMapper;
using Chente.DataAccess.Repositories;
using Chente.DataAccess.Services;
using Chente.Domain.Exceptions;
using System.Collections.ObjectModel;
using System.Windows;

namespace Chente.Desktop.Services;

internal class InstallmentStoreService
{
    private readonly IMapper mapper;
    private readonly ObservableCollection<Domain.Models.Installment> installments = [];
    private readonly InstallmentRepository installmentRepository;
    private readonly LoanStoreService loanStoreService;
    private Domain.Models.Installment? selectedInstallment;

    public IEnumerable<Domain.Models.Installment> Installments => installments;
    public Domain.Models.Installment? SelectedInstallment
    {
        get => selectedInstallment;
        set
        {
            selectedInstallment = value;
            SelectedInstallmentChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler SelectedInstallmentChanged = default!;
    public event EventHandler InstallmentsCollectionChanged = default!;

    public InstallmentStoreService(InstallmentRepository installmentRepository, IMapper mapper, LoanStoreService loanStoreService)
    {
        this.mapper = mapper;
        this.installmentRepository = installmentRepository;
        this.loanStoreService = loanStoreService;
        this.loanStoreService.SelectedLoanChanged += OnSelectedLoanChanged;
        GetAsync().GetAwaiter();
    }

    private void OnSelectedLoanChanged(object? sender, Domain.Models.Loan e)
    {
        GetAsync().GetAwaiter();
    }

    private async Task GetAsync()
    {
        IEnumerable<DataAccess.Models.Installment> installmentsFromDatabase = [];
        IEnumerable<DataAccess.Models.Loan> loansFromDatabase = [];
        if (loanStoreService.SelectedLoan is not null)
        {
            DataAccess.Models.Loan selectedLoan = mapper.Map<DataAccess.Models.Loan>(loanStoreService.SelectedLoan);
            selectedLoan.Id = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(loanStoreService.SelectedLoan.LoanNumber);
            installmentsFromDatabase = await installmentRepository.GetAsync(selectedLoan);
        }
        else
        {
            installmentsFromDatabase = await installmentRepository.GetAsync();
        }
        IEnumerable<Domain.Models.Installment> installments = mapper.Map<IEnumerable<Domain.Models.Installment>>(installmentsFromDatabase);
        if (this.installments.Any())
        {
            this.installments.Clear();
        }
        foreach (var installment in installments)
        {
            this.installments.Add(installment);
        }
        InstallmentsCollectionChanged?.Invoke(this, EventArgs.Empty);
        SelectedInstallment = null!;
        SelectedInstallmentChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PayInstallmentAsync(Domain.Models.Installment installment, decimal amountOfPayment, DateTime dateOfPayment)
    {
        try
        {
            decimal balanceOverAmountDue = installment.Pay(amountOfPayment, dateOfPayment);
            await installmentRepository.UpdateAsync(new DataAccess.Models.Installment
            {
                Id = DatabaseKeyManager.GetPrimaryKeyFrom(installment.InstallmentNumber),
                DateDue = installment.DateDue,
                Amount = installment.Amount,
                AmountPaid = installment.AmountPaid,
                DatePaid = installment.DatePaid,
                BeginningBalance = installment.BeginningBalance,
                EndingBalance = installment.EndingBalance,
            });
            if (balanceOverAmountDue > 0)
            {
                MessageBox.Show($"Installment paid. Amount returned to borrower: {balanceOverAmountDue}");
            }
            await GetAsync();
        }
        catch (InvalidPaymentException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
