using AutoMapper;
using Chente.DataAccess.Repositories;
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

    public BorrowerStoreService(BorrowerRepository borrowerRepository, IMapper mapper)
    {
        this.borrowerRepository = borrowerRepository;
        this.mapper = mapper;
        GetAsync().GetAwaiter();
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
    }

    internal async Task DeleteAsync(int id)
    {
        await borrowerRepository.DeleteAsync(id);
        await GetAsync();
    }
}
