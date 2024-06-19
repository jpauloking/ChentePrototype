using AutoMapper;

namespace Chente.Desktop.AutoMapperProfiles;

internal class BorrowerMapper
{
    private readonly IMapper mapper;

    public BorrowerMapper(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public DataAccess.Models.Borrower MapToDataAccessBorrower(ViewModels.BorrowerViewModel borrower)
    {
        return mapper.Map<DataAccess.Models.Borrower>(borrower);
    }

    public DataAccess.Models.Borrower MapToDataAccessBorrower(Domain.Models.Borrower borrower)
    {
        return mapper.Map<DataAccess.Models.Borrower>(borrower);
    }

    public Domain.Models.Borrower MapToDomainBorrower(ViewModels.BorrowerViewModel borrower)
    {
        return mapper.Map<Domain.Models.Borrower>(borrower);
    }

    public Domain.Models.Borrower MapToDomainBorrower(DataAccess.Models.Borrower borrower)
    {
        return mapper.Map<Domain.Models.Borrower>(borrower);
    }

    public ViewModels.BorrowerViewModel MapToViewModelBorrower(DataAccess.Models.Borrower borrower)
    {
        return mapper.Map<ViewModels.BorrowerViewModel>(borrower);
    }

    public ViewModels.BorrowerViewModel MapToViewModelBorrower(Domain.Models.Borrower borrower)
    {
        return mapper.Map<ViewModels.BorrowerViewModel>(borrower);
    }
}
