using AutoMapper;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal partial class BorrowerStoreService
{
    private readonly IMapper mapper;
    private readonly BorrowerRepository borrowerRepository;
    private readonly ObservableCollection<Domain.Models.Borrower> borrowers = [];
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

    public BorrowerStoreService(IMapper mapper, BorrowerRepository borrowerRepository)
    {
        this.mapper = mapper;
        this.borrowerRepository = borrowerRepository;
        GetAsync().GetAwaiter();
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

    public async Task CreateAsync(Domain.Models.Borrower borrower)
    {
        DataAccess.Models.Borrower borrowerToSaveInDatabase = mapper.Map<DataAccess.Models.Borrower>(borrower);
        await borrowerRepository.CreateAsync(borrowerToSaveInDatabase);
        await GetAsync();
        SelectedBorrower = mapper.Map<Domain.Models.Borrower>(borrowerToSaveInDatabase);
    }

    public async Task DeleteAsync(int id)
    {
        await borrowerRepository.DeleteAsync(id);
        await GetAsync();
    }

    internal async Task UpdateAsync(Domain.Models.Borrower borrower)
    {
        DataAccess.Models.Borrower borrowerToSaveInDatabase = mapper.Map<DataAccess.Models.Borrower>(borrower);
        borrowerToSaveInDatabase.Id = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrower.BorrowerNumber);
        await borrowerRepository.UpdateAsync(borrowerToSaveInDatabase);
        await GetAsync();
        SelectedBorrower = mapper.Map<Domain.Models.Borrower>(borrowerToSaveInDatabase);
    }
}
