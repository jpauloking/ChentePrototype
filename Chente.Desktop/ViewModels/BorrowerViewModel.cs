using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowerViewModel : ViewModelBase
{
    [ObservableProperty]
    private string borrowerNumber = null!;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayName))]
    private string firstName = null!;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayName))]
    private string lastName = null!;
    [ObservableProperty]
    private string emailAddress = null!;
    [ObservableProperty]
    private string? phoneNumber;
    public string DisplayName => $"{FirstName} {LastName}";
}
