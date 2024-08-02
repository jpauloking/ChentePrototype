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
    private readonly IMapper mapper;
    [ObservableProperty]
    private bool hasSelectedInstallment;

    public InstallmentViewModel InstallmentViewModel => mapper.Map<InstallmentViewModel>(installmentStoreService.SelectedInstallment);

    public InstallmentDetailsViewModel(InstallmentStoreService installmentStoreService, IMapper mapper)
    {
        this.installmentStoreService = installmentStoreService;
        this.mapper = mapper;
        this.installmentStoreService.SelectedInstallmentChanged += OnSelectedInstallmentChanged;
    }

    private void OnSelectedInstallmentChanged(object? sender, EventArgs e)
    {
        HasSelectedInstallment = installmentStoreService.SelectedInstallment is not null;
        OnPropertyChanged(nameof(InstallmentViewModel));
        OnPropertyChanged(nameof(HasSelectedInstallment));
    }
}
