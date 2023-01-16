﻿using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Unowhy_Tools_WPF.Services.Contracts;
using Wpf.Ui.Mvvm.Contracts;

namespace Unowhy_Tools_WPF.ViewModels;

public class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;

    private readonly ITestWindowService _testWindowService;

    private ICommand _navigateCommand;

    private ICommand _openWindowCommand;

    public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

    public ICommand OpenWindowCommand => _openWindowCommand ??= new RelayCommand<string>(OnOpenWindow);

    public DashboardViewModel(INavigationService navigationService, ITestWindowService testWindowService)
    {
        _navigationService = navigationService;
        _testWindowService = testWindowService;
    }

    public void OnNavigatedTo()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Unowhy_Tools_WPF");
    }

    public void OnNavigatedFrom()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Unowhy_Tools_WPF");
    }

    private void OnNavigate(string parameter)
    {
        switch (parameter)
        {
            case "navigate_to_winre":
                _navigationService.Navigate(typeof(Views.Pages.WinRE));
                return;
        }
    }

    private void OnOpenWindow(string parameter)
    {
        switch (parameter)
        {
            /*case "open_window_store":
                _testWindowService.Show<Views.Windows.StoreWindow>();
                return;

            case "open_window_manager":
                _testWindowService.Show<Views.Windows.TaskManagerWindow>();
                return;

            case "open_window_editor":
                _testWindowService.Show<Views.Windows.EditorWindow>();
                return;

            case "open_window_settings":
                _testWindowService.Show<Views.Windows.SettingsWindow>();
                return;

            case "open_window_experimental":
                _testWindowService.Show<Views.Windows.ExperimentalWindow>();
                return;*/
        }
    }
}
