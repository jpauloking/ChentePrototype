using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Services;
using AutoMapper;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowerFormViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly WindowManager windowManager;
    private readonly BorrowerStoreService borrowerStoreService;

    public BorrowerViewModel SelectedBorrower => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

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
            var borrower = new Domain.Models.Borrower(firstName, lastName, emailAddress, phoneNumber);

            if (SelectedBorrower is not null)
            {
                borrower.BorrowerNumber = SelectedBorrower.BorrowerNumber;
                await borrowerStoreService.UpdateAsync(borrower);
            }
            else
            {
                await borrowerStoreService.CreateAsync(borrower);
            }

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

    public BorrowerFormViewModel(WindowManager windowManager, BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
        this.windowManager = windowManager;
        this.borrowerStoreService = borrowerStoreService;
        this.mapper = mapper;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
        if (SelectedBorrower is not null)
        {
            FirstName = SelectedBorrower.FirstName;
            LastName = SelectedBorrower.LastName;
            EmailAddress = SelectedBorrower.EmailAddress;
            PhoneNumber = SelectedBorrower.PhoneNumber;
        }
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        if (SelectedBorrower is not null)
        {
            FirstName = SelectedBorrower.FirstName;
            LastName = SelectedBorrower.LastName;
            EmailAddress = SelectedBorrower.EmailAddress;
            PhoneNumber = SelectedBorrower.PhoneNumber;
            OnPropertyChanged(nameof(SelectedBorrower));
        }
    }
}
