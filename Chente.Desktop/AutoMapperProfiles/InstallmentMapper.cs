using AutoMapper;

namespace Chente.Desktop.AutoMapperProfiles;

internal class InstallmentMapper
{
    private readonly IMapper mapper;

    public InstallmentMapper(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public DataAccess.Models.Installment MapToDataAccessBorrower(ViewModels.InstallmentViewModel installment)
    {
        return mapper.Map<DataAccess.Models.Installment>(installment);
    }

    public DataAccess.Models.Installment MapToDataAccessBorrower(Domain.Models.Installment installment)
    {
        return mapper.Map<DataAccess.Models.Installment>(installment);
    }

    public Domain.Models.Installment MapToDomainBorrower(ViewModels.InstallmentViewModel installment)
    {
        return mapper.Map<Domain.Models.Installment>(installment);
    }

    public Domain.Models.Installment MapToDomainBorrower(DataAccess.Models.Installment installment)
    {
        return mapper.Map<Domain.Models.Installment>(installment);
    }

    public ViewModels.InstallmentViewModel MapToViewModelBorrower(DataAccess.Models.Installment installment)
    {
        return mapper.Map<ViewModels.InstallmentViewModel>(installment);
    }

    public ViewModels.InstallmentViewModel MapToViewModelBorrower(Domain.Models.Installment installment)
    {
        return mapper.Map<ViewModels.InstallmentViewModel>(installment);
    }
}
