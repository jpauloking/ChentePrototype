﻿using AutoMapper;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;
using Chente.Desktop.Extensions.ModelExtensions;

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

    public async Task AddLoanToBorrower(Domain.Models.Loan loan)
    {
        int selectedBorrowerId = DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(SelectedBorrower!.BorrowerNumber);
        // Get borrower from database because Selected Borrower does not contain list of borrower's loans which is needed to validate outstanding loans business rule
        DataAccess.Models.Borrower? borrowerFromDatabase = await borrowerRepository.GetAsync(selectedBorrowerId);
        //Cannot use Automapper because of infinite recursion: Domain.Models.Borrower borrower = mapper.Map<Domain.Models.Borrower>(dataAccessBorrower); throws Overflow exception.

        if (borrowerFromDatabase is not null)
        {
            Domain.Models.Borrower borrower = borrowerFromDatabase.MapToDomainBorrower();

            try
            {
                borrower.AddLoan(mapper.Map<Domain.Models.Loan>(loan));
                DataAccess.Models.Borrower borrowerToSaveInDatabase = mapper.Map<DataAccess.Models.Borrower>(borrower);
                borrowerToSaveInDatabase.Id = selectedBorrowerId;
                await borrowerRepository.UpdateAsync(borrowerToSaveInDatabase);
                // Todo - Add LoanNumber to borrower's loans before assigning to selected borrower
                //SelectedBorrower = mapper.Map<Domain.Models.Borrower>(borrowerToSaveInDatabase); Automapper throws overfloe exception
                SelectedBorrower = borrowerToSaveInDatabase.MapToDomainBorrower();
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
