using Chente.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Chente.Desktop.Services;

internal class ViewModelLocator
{
    private readonly IServiceProvider serviceProvider;

    public ViewModelLocator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public MainViewModel MainViewModel => serviceProvider.GetRequiredService<MainViewModel>();

}
