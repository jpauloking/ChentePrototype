using AutoMapper;
using Chente.Desktop.Controls.Borrower.BorrowerList;
using Chente.Desktop.Core;
using Chente.Desktop.Extensions.ModelExtensions;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class BorrowersViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly BorrowerStoreService borrowerStoreService;
    private readonly BorrowerListViewModel borrowerListViewModel;
    private readonly BorrowerDetailsViewModel borrowerDetailsViewModel;
    private readonly BorrowerFormViewModel borrowerFormViewModel;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddBorrowerCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditBorrowerCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteBorrowerCommand))]
    private bool hasSelectedBorrower = false;
    [ObservableProperty]
    private int selectedIndex;

    public BorrowerListViewModel BorrowerListViewModel => borrowerListViewModel;
    public BorrowerDetailsViewModel BorrowerDetailsViewModel => borrowerDetailsViewModel;
    public BorrowerFormViewModel BorrowerFormViewModel => borrowerFormViewModel;
    public IEnumerable<BorrowerViewModel> Borrowers => borrowerStoreService.Borrowers.Select(b => b.MapToBorrowerViewModel(mapper));

    public string? SearchPhrase
    {
        get => borrowerStoreService.SearchPhrase;
        set => borrowerStoreService.SearchPhrase = value;
    }
    public BorrowerViewModel? SelectedBorrower
    {
        get => borrowerStoreService.SelectedBorrower?.MapToBorrowerViewModel(mapper)!;
        set => borrowerStoreService.SelectedBorrower = value?.MapToDomainBorrower(mapper);
    }

    public BorrowersViewModel(IMapper mapper, BorrowerStoreService borrowerStoreService, BorrowerListViewModel borrowerListViewModel, BorrowerDetailsViewModel borrowerDetailsViewModel, BorrowerFormViewModel borrowerFormViewModel)
    {
        this.mapper = mapper;
        this.borrowerStoreService = borrowerStoreService;
        this.borrowerListViewModel = borrowerListViewModel;
        this.borrowerDetailsViewModel = borrowerDetailsViewModel;
        this.borrowerFormViewModel = borrowerFormViewModel;
        this.borrowerStoreService.SelectedBorrowerChanged += OnSelectedBorrowerChanged;
    }

    private void OnSelectedBorrowerChanged(object? sender, Domain.Models.Borrower e)
    {
        OnPropertyChanged(nameof(SelectedBorrower));
        SelectedIndex = Borrowers.ToList().IndexOf(SelectedBorrower!);
        HasSelectedBorrower = SelectedBorrower is not null;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnNewBorrower))]
    private void AddBorrower()
    {
        if (borrowerStoreService.SelectedBorrower is not null)
        {
            borrowerStoreService.SelectedBorrower = null;
        }
        BorrowerFormViewModel.ShowBorrowerForm = true;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnSelectedBorrower))]
    private void EditBorrower()
    {
        if (borrowerStoreService.SelectedBorrower is not null)
        {
            BorrowerFormViewModel.ShowBorrowerForm = true;
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOnSelectedBorrower))]
    private async Task DeleteBorrower()
    {
        if (SelectedBorrower is not null)
        {
            MessageBoxResult userResponse = MessageBox.Show($"Are you sure you want to delete borrower? Borrower Number: {SelectedBorrower.BorrowerNumber} Name: {SelectedBorrower.DisplayName} will be deleted permanently. THIS ACTION IS NOT REVERSIBLE", "System caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (userResponse == MessageBoxResult.OK)
            {
                await borrowerStoreService.DeleteAsync(DataAccess.Services.DatabaseKeyManager.GetPrimaryKeyFrom(borrowerStoreService.SelectedBorrower!.BorrowerNumber));
            }
            else
            {
                MessageBox.Show("Action cancelled.", "System says", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("Please select a borrower and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SearchPhrase = string.Empty;
        OnPropertyChanged(nameof(SearchPhrase));
        SelectedBorrower = null;
        BorrowerFormViewModel.ShowBorrowerForm = false;
    }

    private bool CanExecuteOnSelectedBorrower()
    {
        return HasSelectedBorrower;
    }

    private bool CanExecuteOnNewBorrower()
    {
        return !HasSelectedBorrower;
    }
}
