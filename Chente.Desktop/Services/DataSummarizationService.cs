using AutoMapper;
using Chente.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Chente.Desktop.Services;

internal class DataSummarizationService
{
    private readonly IMapper mapper;
    private readonly InstallmentRepository installmentRepository;
    private readonly LoanRepository loanRepository;
    private readonly BorrowerRepository borrowerRepository;

    public ObservableCollection<Domain.Models.Borrower> Borrowers = [];
    public ObservableCollection<Domain.Models.Installment> Installments = [];
    public ObservableCollection<Domain.Models.Loan> Loans = [];

    public DataSummarizationService(IMapper mapper, InstallmentRepository installmentRepository, LoanRepository loanRepository, BorrowerRepository borrowerRepository)
    {
        this.mapper = mapper;
        this.installmentRepository = installmentRepository;
        this.loanRepository = loanRepository;
        this.borrowerRepository = borrowerRepository;
        LoadData();
    }

    public void Refresh()
    {
        LoadData();
    }

    private void LoadData()
    {
        GetBorrowersAsync().Wait();
        GetLoansAsync().Wait();
        GetInstallmentsAsync().Wait();
    }

    private async Task GetBorrowersAsync()
    {
        var borrowers = await borrowerRepository.GetAsync();
        if (Borrowers.Any())
        {
            Borrowers.Clear();
        }
        foreach (var borrower in mapper.Map<IEnumerable<Domain.Models.Borrower>>(borrowers))
        {
            Borrowers.Add(borrower);
        }
    }

    private async Task GetLoansAsync()
    {
        var loans = await loanRepository.GetAsync();
        if (Loans.Any())
        {
            Loans.Clear();
        }
        foreach (var loan in mapper.Map<IEnumerable<Domain.Models.Loan>>(loans))
        {
            Loans.Add(loan);
        }
    }

    private async Task GetInstallmentsAsync()
    {
        var installments = await installmentRepository.GetAsync();
        if (Installments.Any())
        {
            Installments.Clear();
        }
        foreach (var installment in mapper.Map<IEnumerable<Domain.Models.Installment>>(installments))
        {
            Installments.Add(installment);
        }
    }
}
