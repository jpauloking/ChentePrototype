using AutoMapper;

namespace Chente.Desktop.ModelMappers;

class BorrowerProfile : Profile
{
    public BorrowerProfile()
    {
        CreateMap<Domain.Models.Borrower, DataAccess.Models.Borrower>();
        CreateMap<DataAccess.Models.Borrower, Domain.Models.Borrower>();
        CreateMap<Domain.Models.Borrower, ViewModels.BorrowerViewModel>();
        CreateMap<ViewModels.BorrowerViewModel, Domain.Models.Borrower>();
    }
}
