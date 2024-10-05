using AutoMapper;

namespace Chente.Desktop.Profiles;

class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<DataAccess.Models.User, ViewModels.UserViewModel>();
        CreateMap<ViewModels.UserViewModel, DataAccess.Models.User>();
    }
}
