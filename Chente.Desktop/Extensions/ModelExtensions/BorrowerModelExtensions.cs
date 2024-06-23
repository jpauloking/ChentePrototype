using AutoMapper;

namespace Chente.Desktop.Extensions.ModelExtensions;

internal static class BorrowerModelExtensions
{
    public static Domain.Models.Borrower MapToDomainBorrower(this DataAccess.Models.Borrower dataAccessBorrower)
    {
        List<Domain.Models.Loan> selectedBorrowerLoans = [];

        foreach (DataAccess.Models.Loan loanFromDatabase in dataAccessBorrower.Loans)
        {
            List<Domain.Models.Installment> selectedBorrowerInstallments = [];

            foreach (var installmentFromDatabase in loanFromDatabase.Installments)
            {
                selectedBorrowerInstallments.Add(new Domain.Models.Installment(installmentFromDatabase.InstallmentNumber, installmentFromDatabase.DateDue, installmentFromDatabase.Amount, installmentFromDatabase.BeginningBalance, installmentFromDatabase.EndingBalance, installmentFromDatabase.AmountPaid));
            }

            selectedBorrowerLoans.Add(new Domain.Models.Loan(loanFromDatabase.LoanNumber, loanFromDatabase.DateOpened, loanFromDatabase.Principal, loanFromDatabase.InterestRate, loanFromDatabase.DurationInDays, loanFromDatabase.AmountPerInstallment, selectedBorrowerInstallments));
        }

        Domain.Models.Borrower borrower = new Domain.Models.Borrower(dataAccessBorrower.BorrowerNumber, dataAccessBorrower.FirstName, dataAccessBorrower.LastName, dataAccessBorrower.EmailAddress, dataAccessBorrower.PhoneNumber, selectedBorrowerLoans);
        return borrower;
    }

    public static ViewModels.BorrowerViewModel MapToBorrowerViewModel(this Domain.Models.Borrower borrower, IMapper mapper)
    {
        return mapper.Map<ViewModels.BorrowerViewModel>(borrower);
    }

    public static Domain.Models.Borrower MapToDomainBorrower(this ViewModels.BorrowerViewModel borrowerViewModel, IMapper mapper)
    {
        return mapper.Map<Domain.Models.Borrower>(borrowerViewModel);
    }
}
