using Chente.Desktop.Core;
using System.Windows;

namespace Chente.Desktop.Services;

internal class WindowManager
{
    private readonly WindowMapper windowMapper;
    private readonly Func<Type, ViewModelBase> viewModelFactory;

    public WindowManager(WindowMapper windowMapper, Func<Type, ViewModelBase> viewModelFactory)
    {
        this.windowMapper = windowMapper;
        this.viewModelFactory = viewModelFactory;
    }

    public void ShowWindow<TViewModel>() where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = viewModelFactory?.Invoke(typeof(TViewModel));
        if (viewModel is null)
        {
            throw new Exception($"View for view model of type {typeof(TViewModel)} was not found");
        }
        var windowType = windowMapper.GetWindowType(viewModel.GetType());
        Window window = (Activator.CreateInstance(windowType) as Window)!;
        window.DataContext = viewModel;
        window.Show();
    }

    public void ShowWindow(ViewModelBase viewModel)
    {
        var windowType = windowMapper.GetWindowType(viewModel.GetType());
        Window window = (Activator.CreateInstance(windowType) as Window)!;
        window.DataContext = viewModel;
        window.Show();
    }

    public void ShowModal<TViewModel>() where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = viewModelFactory?.Invoke(typeof(TViewModel));
        if (viewModel is null)
        {
            throw new Exception($"View for view model of type {typeof(TViewModel)} was not found");
        }
        var windowType = windowMapper.GetWindowType(viewModel.GetType());
        Window window = (Activator.CreateInstance(windowType) as Window)!;
        window.DataContext = viewModel;
        window.Owner = App.Current.MainWindow;
        window.WindowStartupLocation= WindowStartupLocation.CenterOwner;
        window.ShowDialog();
    }

    public void CloseModal<TViewModel>() where TViewModel : ViewModelBase
    {
        Window mainWindow = App.Current.MainWindow;
        WindowCollection ownedWindows = mainWindow.OwnedWindows;
        if (ownedWindows.Count > 0)
        {
            (ownedWindows[0] as Window).Close();
        }
    }
}
