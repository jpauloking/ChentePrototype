using AutoMapper;

namespace Chente.Desktop.ModelMappers;

class InstallmentProfile :Profile
{
    public InstallmentProfile()
    {
        CreateMap<Domain.Models.Installment, DataAccess.Models.Installment>();
        CreateMap<DataAccess.Models.Installment, Domain.Models.Installment>();
        CreateMap<Domain.Models.Installment, ViewModels.InstallmentViewModel>().ForMember(i => i.LoanNumber, i => i.MapFrom(i => i.Loan.LoanNumber));
        CreateMap<ViewModels.InstallmentViewModel, Domain.Models.Installment>();
    }
}
