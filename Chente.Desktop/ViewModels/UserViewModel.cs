using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chente.Desktop.ViewModels;

internal partial class UserViewModel : ViewModelBase
{
    [ObservableProperty]
    [Required]
    private string username = null!;
    [ObservableProperty]
    [Required]
    private string password = null!;
    [ObservableProperty]
    [EmailAddress]
    private string emailAddress = null!;
    [ObservableProperty]
    [Phone]
    private string? phoneNumber;
    [ObservableProperty]
    [Required]
    private string role = null!;
}
