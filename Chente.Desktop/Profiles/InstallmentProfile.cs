using AutoMapper;

namespace Chente.Desktop.Profiles;

class InstallmentProfile :Profile
{
    public InstallmentProfile()
    {
        CreateMap<Domain.Models.Installment, DataAccess.Models.Installment>();
        CreateMap<DataAccess.Models.Installment, Domain.Models.Installment>();
        CreateMap<Domain.Models.Installment, ViewModels.InstallmentViewModel>().ForMember(dest => dest.LoanNumber, opt => opt.MapFrom(src => src.Loan.LoanNumber));
        CreateMap<ViewModels.InstallmentViewModel, Domain.Models.Installment>();
    }
}
