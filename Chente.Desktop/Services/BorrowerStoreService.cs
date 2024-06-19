using AutoMapper;
using Chente.DataAccess.Models;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal partial class BorrowerStoreService
{
    private readonly BorrowerRepository borrowerRepository;
    private readonly ObservableCollection<Domain.Models.Borrower> borrowers = [];
    private readonly IMapper mapper;
    private Domain.Models.Borrower? selectedBorrower;
    private string? searchPhrase = null!;

    public IEnumerable<Domain.Models.Borrower> Borrowers => borrowers;
    public Domain.Models.Borrower? SelectedBorrower
    {
        get => selectedBorrower;
        set
        {
            selectedBorrower = value;
            SelectedBorrowerChanged?.Invoke(this, value!);
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
    public event EventHandler<Domain.Models.Borrower> SelectedBorrowerChanged = default!;
    public event EventHandler BorrowersCollectionChanged = default!;

    public BorrowerStoreService(BorrowerRepository borrowerRepository, IMapper mapper)
    {
        this.borrowerRepository = borrowerRepository;
        this.mapper = mapper;
        GetAsync().GetAwaiter();
    }

    public async Task AddLoanToBorrower(DataAccess.Models.Loan loan)
    {
        // Todo - Fix the overflow exception caused by automapper recursive call while mapping the EFCore model and the domain model
        //DataAccess.Models.Borrower selectedBorrower = mapper.Map<DataAccess.Models.Borrower>(SelectedBorrower);
        int selectedBorrowerId = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(SelectedBorrower!.BorrowerNumber);
        DataAccess.Models.Borrower? borrowerFromDatabase = await borrowerRepository.GetAsync(selectedBorrowerId);
        Domain.Models.Borrower borrower = mapper.Map<Domain.Models.Borrower>(borrowerFromDatabase);
        borrower.AddLoan(mapper.Map<Domain.Models.Loan>(loan));
        DataAccess.Models.Borrower borrowerToSaveInDatabase = mapper.Map<DataAccess.Models.Borrower>(borrower);
        await borrowerRepository.UpdateAsync(borrowerToSaveInDatabase);
    }

    private async Task GetAsync()
    {
        IEnumerable<DataAccess.Models.Borrower> borrowersFromDatabase = await borrowerRepository.GetAsync();
        IEnumerable<Domain.Models.Borrower> borrowers = mapper.Map<IEnumerable<Domain.Models.Borrower>>(borrowersFromDatabase);
        if (!string.IsNullOrEmpty(SearchPhrase))
        {
            borrowers = borrowers.Where(b => b.BorrowerNumber.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase) ||
            b.FirstName.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase) ||
            b.LastName.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase) ||
            b.EmailAddress.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase) ||
            b.PhoneNumber!.Contains(SearchPhrase, StringComparison.InvariantCultureIgnoreCase));
        }
        if (this.borrowers.Any())
        {
            this.borrowers.Clear();
        }
        foreach (var borrower in borrowers)
        {
            this.borrowers.Add(borrower);
        }
        BorrowersCollectionChanged?.Invoke(this, EventArgs.Empty);
        SelectedBorrower = null;
        SelectedBorrowerChanged?.Invoke(this, null!);
    }

    public async Task CreateAsync(DataAccess.Models.Borrower borrower)
    {
        await borrowerRepository.CreateAsync(borrower);
        await GetAsync();
        SelectedBorrower = mapper.Map<Domain.Models.Borrower>(borrower);
    }

    public async Task DeleteAsync(int id)
    {
        await borrowerRepository.DeleteAsync(id);
        await GetAsync();
    }

    internal async Task UpdateAsync(Borrower borrower)
    {
        await borrowerRepository.UpdateAsync(borrower);
        await GetAsync();
        SelectedBorrower = mapper.Map<Domain.Models.Borrower>(borrower);
    }
}
