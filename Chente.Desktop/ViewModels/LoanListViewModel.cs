using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal class LoanListViewModel : ViewModelBase
{
    private readonly LoanStoreService loanStoreService;
    private readonly IMapper mapper;

    public IEnumerable<LoanViewModel> Loans => mapper.Map<IEnumerable<LoanViewModel>>(loanStoreService.Loans);

    public LoanListViewModel(LoanStoreService loanStoreService, IMapper mapper)
    {
        this.loanStoreService = loanStoreService;
        this.mapper = mapper;
    }
}
