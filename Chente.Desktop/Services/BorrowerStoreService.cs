using AutoMapper;
using Chente.DataAccess.Repositories;
using Chente.Domain.Exceptions;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal partial class BorrowerStoreService
{
    private readonly BorrowerRepository borrowerRepository;
    private readonly ObservableCollection<Domain.Models.Borrower> borrowers = [];
    private readonly IMapper mapper;
    private Domain.Models.Borrower? selectedBorrower;

    public IEnumerable<Domain.Models.Borrower> Borrowers => borrowers;
    public Domain.Models.Borrower? SelectedBorrower
    {
        get => selectedBorrower;
        set
        {
            if (value is not null)
            {
                selectedBorrower = value;
                SelectedBorrowerChanged?.Invoke(this, value);
            }
        }
    }
    public event EventHandler<Domain.Models.Borrower> SelectedBorrowerChanged = default!;
    public event EventHandler BorrowerAdded = default!;

    public BorrowerStoreService(BorrowerRepository borrowerRepository, IMapper mapper)
    {
        this.borrowerRepository = borrowerRepository;
        this.mapper = mapper;
        GetAsync().GetAwaiter();
    }

    public async Task AddLoanToBorrower(DataAccess.Models.Loan loan)
    {
        DataAccess.Models.Borrower selectedBorrower = mapper.Map<DataAccess.Models.Borrower>(SelectedBorrower);
        selectedBorrower.Id = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(SelectedBorrower!.BorrowerNumber);
        selectedBorrower.Loans.Add(loan);
        await borrowerRepository.UpdateAsync(selectedBorrower);
    }

    private async Task GetAsync()
    {
        IEnumerable<DataAccess.Models.Borrower> borrowersFromDatabase = await borrowerRepository.GetAsync();
        IEnumerable<Domain.Models.Borrower> borrowers = mapper.Map<IEnumerable<Domain.Models.Borrower>>(borrowersFromDatabase);
        if (this.borrowers.Any())
        {
            this.borrowers.Clear();
        }
        foreach (var borrower in borrowers)
        {
            this.borrowers.Add(borrower);
        }
    }

    public async Task CreateAsync(DataAccess.Models.Borrower borrower)
    {
        await borrowerRepository.CreateAsync(borrower);
        await GetAsync();
        BorrowerAdded?.Invoke(this, EventArgs.Empty);
    }

    public async Task DeleteAsync(int id)
    {
        await borrowerRepository.DeleteAsync(id);
        await GetAsync();
    }
}
