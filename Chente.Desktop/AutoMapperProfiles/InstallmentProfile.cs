using AutoMapper;

namespace Chente.Desktop.AutoMapperProfiles;

class InstallmentProfile :Profile
{
    public InstallmentProfile()
    {
        CreateMap<Domain.Models.Installment, DataAccess.Models.Installment>();
        CreateMap<DataAccess.Models.Installment, Domain.Models.Installment>();
    }
}
