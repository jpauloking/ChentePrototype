using AutoMapper;

namespace Chente.Desktop.AutoMapperProfiles;

internal class LoanMapper
{
    private readonly IMapper mapper;

    public LoanMapper(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public DataAccess.Models.Loan MapToDataAccessBorrower(ViewModels.LoanViewModel loan)
    {
        return mapper.Map<DataAccess.Models.Loan>(loan);
    }

    public DataAccess.Models.Loan MapToDataAccessBorrower(Domain.Models.Loan loan)
    {
        return mapper.Map<DataAccess.Models.Loan>(loan);
    }

    public Domain.Models.Loan MapToDomainBorrower(ViewModels.LoanViewModel loan)
    {
        return mapper.Map<Domain.Models.Loan>(loan);
    }

    public Domain.Models.Loan MapToDomainBorrower(DataAccess.Models.Loan loan)
    {
        return mapper.Map<Domain.Models.Loan>(loan);
    }

    public ViewModels.LoanViewModel MapToViewModelBorrower(DataAccess.Models.Loan loan)
    {
        return mapper.Map<ViewModels.LoanViewModel>(loan);
    }

    public ViewModels.LoanViewModel MapToViewModelBorrower(Domain.Models.Loan loan)
    {
        return mapper.Map<ViewModels.LoanViewModel>(loan);
    }
}
