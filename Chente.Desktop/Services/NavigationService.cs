using Chente.Desktop.Core;

namespace Chente.Desktop.Services;

public class NavigationService
{
    private ViewModelBase? currentViewModel;
    private readonly Func<Type, ViewModelBase> viewModelFactory;

    public event EventHandler? CurrentViewModelChanged;

    public ViewModelBase? CurrentViewModel
    {
        get
        {
            return currentViewModel;
        }
        private set
        {
            currentViewModel = value;
            CurrentViewModelChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        this.viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = viewModelFactory?.Invoke(typeof(TViewModel));
        if (viewModel is null)
        {
            throw new Exception($"View for view model of type {typeof(TViewModel)} was not found");
        }
        CurrentViewModel = viewModel;
    }

}
