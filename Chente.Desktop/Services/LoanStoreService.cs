using AutoMapper;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal class LoanStoreService
{
    private readonly LoanRepository loanRepository;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly ObservableCollection<Domain.Models.Loan> loans = [];
    private readonly IMapper mapper;
    private Domain.Models.Loan? selectedLoan;

    public IEnumerable<Domain.Models.Loan> Loans => loans;
    public Domain.Models.Loan? SelectedLoan
    {
        get => selectedLoan;
        set
        {
            if (value is not null)
            {
                selectedLoan = value;
                SelectedLoanChanged?.Invoke(this, value);
            }
        }
    }
    public event EventHandler<Domain.Models.Loan> SelectedLoanChanged = default!;

    public LoanStoreService(LoanRepository loanRepository, BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.loanRepository = loanRepository;
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
        GetAsync().GetAwaiter();
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        GetAsync().GetAwaiter();
    }

    private async Task GetAsync()
    {
        IEnumerable<DataAccess.Models.Loan> loansFromDatabase = [];
        if (borrowerStoreService.SelectedBorrower is null)
        {
            loansFromDatabase = await loanRepository.GetAsync();
        }
        else
        {
            DataAccess.Models.Borrower selectedBorrower = mapper.Map<DataAccess.Models.Borrower>(borrowerStoreService.SelectedBorrower);
            selectedBorrower.Id = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrowerStoreService.SelectedBorrower!.BorrowerNumber);
            loansFromDatabase = await loanRepository.GetAsync(selectedBorrower);
        }
        IEnumerable<Domain.Models.Loan> loans = mapper.Map<IEnumerable<Domain.Models.Loan>>(loansFromDatabase);
        if (this.loans.Any())
        {
            this.loans.Clear();
        }
        foreach (var loan in loans)
        {
            this.loans.Add(loan);
        }
    }

    public async Task CreateAsync(DataAccess.Models.Loan loan)
    {
        // Todo - Debug error thrown while saving old borrower's loan to the database due to primary key already used, might work for new borrowers though.
        if (loan.Borrower.Id > 0)
        {
            await borrowerStoreService.AddLoanToBorrower(loan);
        }
        else
        {
            await loanRepository.CreateAsync(loan);
            await GetAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        await loanRepository.DeleteAsync(id);
        await GetAsync();
    }
}
