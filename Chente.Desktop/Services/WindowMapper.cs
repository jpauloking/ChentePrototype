using Chente.Desktop.Core;
using Chente.Desktop.ViewModels;
using Chente.Desktop.Views;
using System.Windows;

namespace Chente.Desktop.Services;

internal class WindowMapper
{
    private readonly Dictionary<Type, Type> mappings = [];

    public WindowMapper()
    {
        RegisterMapping<MainViewModel, MainWindow>();
        RegisterMapping<ModalViewModel, ModalWindow>();
    }

    public void RegisterMapping<TViewModel, TWindow>() where TViewModel : ViewModelBase where TWindow : Window
    {
        mappings.Add(typeof(TViewModel), typeof(TWindow));
    }

    public Type GetWindowType(Type viewModelType)
    {
        mappings.TryGetValue(viewModelType, out var windowType);
        if (windowType is null)
        {
            throw new Exception($"No window registered for view model of type {viewModelType}");
        }
        return windowType;
    }
}
