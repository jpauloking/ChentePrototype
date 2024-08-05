using AutoMapper;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal class LoanStoreService
{
    private readonly BorrowerRepository borrowerRepository;
    private readonly LoanRepository loanRepository;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly ObservableCollection<Domain.Models.Loan> loans = [];
    private readonly IMapper mapper;
    private Domain.Models.Loan? selectedLoan;
    private string? searchPhrase = null!;
    private DateTime? startDate = null;
    private DateTime? endDate = null;
    private bool includePaid = false;
    private bool onlyOverdue = false;

    public IEnumerable<Domain.Models.Loan> Loans => loans;
    public Domain.Models.Loan? SelectedLoan
    {
        get => selectedLoan;
        set
        {
            selectedLoan = value;
            SelectedLoanChanged?.Invoke(this, value!);
        }
    }
    public string? SearchPhrase
    {
        get => searchPhrase;
        set
        {
            searchPhrase = value;
            GetAsync().GetAwaiter();
        }
    }
    public DateTime? StartDate
    {
        get => startDate;
        set
        {
            startDate = value;
            GetAsync().GetAwaiter();
        }
    }
    public DateTime? EndDate
    {
        get => endDate;
        set
        {
            endDate = value;
            GetAsync().GetAwaiter();
        }
    }
    public bool IncludePaid
    {
        get => includePaid;
        set
        {
            includePaid = value;
            GetAsync().GetAwaiter();
        }
    }
    public bool OnlyOverdue
    {
        get => onlyOverdue;
        set
        {
            onlyOverdue = value;
            GetAsync().GetAwaiter();
        }
    }

    public event EventHandler<Domain.Models.Loan> SelectedLoanChanged = default!;
    public event EventHandler LoansCollectionChanged = default!;

    public LoanStoreService(BorrowerRepository borrowerRepository, LoanRepository loanRepository, BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.borrowerRepository = borrowerRepository;
        this.loanRepository = loanRepository;
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
        this.borrowerStoreService.BorrowersCollectionChanged += OnBorrowersCollectionChanged;
        GetAsync().GetAwaiter();
    }

    private void OnBorrowersCollectionChanged(object? sender, EventArgs e)
    {
        //GetAsync().GetAwaiter();
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        GetAsync().GetAwaiter();
        Domain.Models.Borrower selectedBorrower = e;
        var loan = borrowerStoreService.SelectedBorrower?.Loans.AsEnumerable().FirstOrDefault(l => l.IsPaid == false);
        SelectedLoan = loan;
        if (selectedBorrower is not null)
        {
            // Filter loans to return only selected borrower's loans.
        }
    }

    private async Task GetAsync()
    {
        IEnumerable<DataAccess.Models.Loan> loansFromDatabase = [];
        if (borrowerStoreService.SelectedBorrower is not null)
        {
            DataAccess.Models.Borrower selectedBorrower = mapper.Map<DataAccess.Models.Borrower>(borrowerStoreService.SelectedBorrower);
            selectedBorrower.Id = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrowerStoreService.SelectedBorrower.BorrowerNumber);
            loansFromDatabase = await loanRepository.GetAsync(selectedBorrower);
        }
        else
        {
            loansFromDatabase = await loanRepository.GetAsync();
        }
        IEnumerable<Domain.Models.Loan> loans = mapper.Map<IEnumerable<Domain.Models.Loan>>(loansFromDatabase);
        if (!string.IsNullOrEmpty(SearchPhrase))
        {
            loans = loans.Where(l => l.LoanNumber.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase));
        }
        if (StartDate is not null)
        {
            loans = loans.Where(l => l.DateOpened >= StartDate);
        }
        if (EndDate is not null)
        {
            loans = loans.Where(l => l.DateOpened <= EndDate);
        }
        if (!IncludePaid)
        {
            loans = loans.Where(l => !l.IsPaid);
        }
        if (OnlyOverdue)
        {
            loans = loans.Where(l => l.IsOverDue);
        }
        if (this.loans.Any())
        {
            this.loans.Clear();
        }
        foreach (var loan in loans)
        {
            this.loans.Add(loan);
        }
        SelectedLoan = null!;
        LoansCollectionChanged?.Invoke(this, EventArgs.Empty);
        //SelectedLoanChanged?.Invoke(this, null!);
    }

    public async Task AddLoan(Domain.Models.Loan loan)
    {
        try
        {
            Domain.Models.Borrower borrower = await CreateSelectedBorrowerWithLoansFromDatabase();
            borrower.AddLoan(loan);
            loan.Borrower = null!; // loan.Borrower is null - To prevent new record from being created for Borrower in database.
            DataAccess.Models.Loan loanToSaveInDatabase = mapper.Map<DataAccess.Models.Loan>(loan);
            int selectedBorrowerId = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrower.BorrowerNumber);
            loanToSaveInDatabase.BorrowerId = selectedBorrowerId;

            await loanRepository.CreateAsync(loanToSaveInDatabase);

            borrowerStoreService.SelectedBorrower = borrower;
        }
        catch (Domain.Exceptions.HasOutstandingLoanException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        await loanRepository.DeleteAsync(id);
        await GetAsync();
    }

    private async Task<Domain.Models.Borrower> CreateSelectedBorrowerWithLoansFromDatabase()
    {
        int selectedBorrowerId = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrowerStoreService.SelectedBorrower!.BorrowerNumber);

        DataAccess.Models.Borrower? borrowerFromDatabase = await borrowerRepository.GetAsync(selectedBorrowerId);
        if (borrowerFromDatabase is not null)
        {
            IEnumerable<DataAccess.Models.Loan> borrowersLoansFromDatabase = await loanRepository.GetAsync(borrowerFromDatabase);

            Domain.Models.Borrower borrower = mapper.Map<Domain.Models.Borrower>(borrowerFromDatabase);

            IEnumerable<Domain.Models.Loan> borrowerLoans = mapper.Map<IEnumerable<Domain.Models.Loan>>(borrowersLoansFromDatabase);

            borrower = new Domain.Models.Borrower(borrower.BorrowerNumber, borrower.FirstName, borrower.LastName, borrower.EmailAddress, borrower.PhoneNumber, borrowerLoans.ToList());
            return borrower;
        }
        throw new InvalidOperationException("No borrower has been selected or selected borrower could not be found in the database.");
    }
}
