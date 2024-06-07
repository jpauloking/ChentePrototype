using Chente.Desktop.Core;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal partial class ModalViewModel : ViewModelBase
{
    private readonly ModalNavigationService navigationService;

    public ModalNavigationService ModalNavigationService => navigationService;

    public ModalViewModel(ModalNavigationService navigationService)
    {
        this.navigationService = navigationService;
        this.navigationService.CurrentViewModelChanged += HandleCurrentViewModelChanged;
    }

    private void HandleCurrentViewModelChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged();
    }
}
