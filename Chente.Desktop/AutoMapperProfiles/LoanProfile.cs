using AutoMapper;

namespace Chente.Desktop.AutoMapperProfiles;

class LoanProfile  :Profile
{
    public LoanProfile()
    {
        CreateMap<Domain.Models.Loan, DataAccess.Models.Loan>();
        CreateMap<DataAccess.Models.Loan, Domain.Models.Loan>();
        CreateMap<Domain.Models.Loan, ViewModels.LoanViewModel>();
        CreateMap<ViewModels.LoanViewModel, Domain.Models.Loan>();
    }
}
