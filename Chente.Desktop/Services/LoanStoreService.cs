using AutoMapper;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal class LoanStoreService
{
    private readonly LoanRepository loanRepository;
    private readonly ObservableCollection<Domain.Models.Loan> loans = [];
    private readonly IMapper mapper;

    public IEnumerable<Domain.Models.Loan> Loans => loans;

    public LoanStoreService(LoanRepository loanRepository, IMapper mapper)
    {
        this.loanRepository = loanRepository;
        this.mapper = mapper;
        GetAsync().GetAwaiter();
    }

    private async Task GetAsync()
    {
        IEnumerable<DataAccess.Models.Loan> loansFromDatabase = await loanRepository.GetAsync();
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
        await loanRepository.CreateAsync(loan);
        await GetAsync();
    }
}
