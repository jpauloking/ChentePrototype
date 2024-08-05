using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class CategorySummaryItem : ViewModelBase
{
    [ObservableProperty]
    private string category = null!;
    [ObservableProperty]
    private int number;
    [ObservableProperty]
    private decimal amount;
}
