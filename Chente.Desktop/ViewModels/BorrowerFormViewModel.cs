using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Services;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowerFormViewModel : ViewModelBase
{
    private readonly WindowManager windowManager;
    private readonly BorrowerStoreService borrowerStoreService;

    private string firstName = null!;

    [Required]
    [MaxLength(50)]
    public string FirstName
    {
        get => firstName;
        set { SetProperty(ref firstName, value, true); }
    }

    private string lastName = null!;

    [Required]
    [MaxLength(50)]
    public string LastName
    {
        get => lastName;
        set { SetProperty(ref lastName, value, true); }
    }

    public string emailAddress = null!;

    [EmailAddress]
    [Required]
    public string EmailAddress
    {
        get => emailAddress;
        set { SetProperty(ref emailAddress, value, true); }
    }

    public string? phoneNumber;

    [Phone]
    public string? PhoneNumber
    {
        get => phoneNumber;
        set { SetProperty(ref phoneNumber, value, true); }
    }

    [RelayCommand]
    private async Task Save()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            MessageBox.Show("Task failed", "System says");
        }
        else
        {
            var borrower = new DataAccess.Models.Borrower { FirstName = firstName, LastName = lastName, EmailAddress = emailAddress, PhoneNumber = phoneNumber };
            await borrowerStoreService.CreateAsync(borrower);
            windowManager.CloseModal<ModalViewModel>();
            MessageBox.Show("Task completed", "System says");

        }
    }

    [RelayCommand]
    private void Cancel()
    {
        windowManager.CloseModal<ModalViewModel>();
        MessageBox.Show("Task cancelled", "System says");
    }

    public BorrowerFormViewModel(WindowManager windowManager, BorrowerStoreService borrowerStoreService)
    {
        this.windowManager = windowManager;
        this.borrowerStoreService = borrowerStoreService;
    }
}
