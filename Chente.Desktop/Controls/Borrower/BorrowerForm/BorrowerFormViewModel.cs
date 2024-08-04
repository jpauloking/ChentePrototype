using Chente.Desktop.Core;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Chente.Desktop.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowerFormViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly BorrowerStoreService borrowerStoreService;

    public BorrowerViewModel SelectedBorrower => mapper.Map<BorrowerViewModel>(borrowerStoreService.SelectedBorrower);

    [ObservableProperty]
    private bool showBorrowerForm;

    [Required]
    [MaxLength(50)]
    [ObservableProperty]
    private string firstName = null!;

    [Required]
    [MaxLength(50)]
    [ObservableProperty]
    private string lastName = null!;

    [EmailAddress]
    [Required]
    [ObservableProperty]
    public string emailAddress = null!;

    [Phone]
    [ObservableProperty]
    public string? phoneNumber;

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
            var borrower = new Domain.Models.Borrower(FirstName, LastName, EmailAddress, PhoneNumber);

            if (SelectedBorrower is not null)
            {
                borrower.BorrowerNumber = SelectedBorrower.BorrowerNumber;
                await borrowerStoreService.UpdateAsync(borrower);
            }
            else
            {
                await borrowerStoreService.CreateAsync(borrower);
            }

            ShowBorrowerForm = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        ShowBorrowerForm = false;
    }

    public BorrowerFormViewModel(BorrowerStoreService borrowerStoreService, IMapper mapper)
    {
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
        }
        else
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = string.Empty;
            PhoneNumber = string.Empty;
            OnPropertyChanged(nameof(SelectedBorrower));
        }
    }
}
