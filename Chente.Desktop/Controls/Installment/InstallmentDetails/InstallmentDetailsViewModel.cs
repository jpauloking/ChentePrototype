using AutoMapper;
using Chente.Desktop.Core;
using Chente.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Chente.Desktop.ViewModels;

internal partial class InstallmentDetailsViewModel : ViewModelBase
{
    private readonly InstallmentStoreService installmentStoreService;
    private readonly ModalNavigationService modalNavigationService;
    private readonly WindowManager windowManager;
    private readonly IMapper mapper;
    [ObservableProperty]
    private bool hasSelectedInstallment;

    public InstallmentViewModel InstallmentViewModel => mapper.Map<InstallmentViewModel>(installmentStoreService.SelectedInstallment);

    public InstallmentDetailsViewModel(InstallmentStoreService installmentStoreService, IMapper mapper, ModalNavigationService modalNavigationService, WindowManager windowManager)
    {
        this.installmentStoreService = installmentStoreService;
        this.mapper = mapper;
        this.modalNavigationService = modalNavigationService;
        this.windowManager = windowManager;
        this.installmentStoreService.SelectedInstallmentChanged += OnSelectedInstallmentChanged;
    }

    [RelayCommand]
    private void PayInstallment()
    {
        if (HasSelectedInstallment)
        {
            modalNavigationService.NavigateTo<InstallmentFormViewModel>();
            windowManager.ShowModal<ModalViewModel>();
        }
        else
        {
            MessageBox.Show("Please select an installment and try again.", "System says", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }

    private void OnSelectedInstallmentChanged(object? sender, EventArgs e)
    {
        HasSelectedInstallment = installmentStoreService.SelectedInstallment is not null;
        OnPropertyChanged(nameof(InstallmentViewModel));
        OnPropertyChanged(nameof(HasSelectedInstallment));
    }
}
