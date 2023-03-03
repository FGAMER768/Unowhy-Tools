﻿




using Wpf.Ui.Common.Interfaces;
using Unowhy_Tools_WPF.ViewModels;
using System.Threading.Tasks;
using Unowhy_Tools_WPF.Views;
using System.Windows;
using System.Diagnostics;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using Unowhy_Tools;
using System.Windows.Media;
using Microsoft.Win32;
using System;

namespace Unowhy_Tools_WPF.Views.Pages;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class About : INavigableView<DashboardViewModel>
{
    private readonly ISnackbarService _snackbarService;

    public DashboardViewModel ViewModel
    {
        get;
    }

    public void applylang()
    {
        ubtnlab.Text = UT.GetLang("udcheck");
    }

    public async void Init(object sender, EventArgs e)
    {
        applylang();
        string ver = "Version " + UT.version.getver() + " (Build " + UT.version.getverbuild().ToString() + ") ";

        if (UT.version.isdeb()) ver = ver + "(Debug/Beta)";
        else ver = ver + "(Release)";

        verlab.Text = ver;

        RegistryKey lcs = Registry.CurrentUser.OpenSubKey(@"Software\STY1001\Unowhy Tools", false);
        string utcuab = lcs.GetValue("UpdateStart").ToString();
        if (utcuab == "1")
        {
            ubtnlab.Text = UT.GetLang("update.check");
            if (await UT.version.newver())
            {
                Color white = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                Color gray = (Color)ColorConverter.ConvertFromString("#bebebe");
                ubtnlab.Text = UT.GetLang("newver");
                ubtnlab.Foreground = new SolidColorBrush(white);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(gray);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(white);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(gray);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(white);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(gray);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(white);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(gray);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(white);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(gray);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(white);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(gray);
                await Task.Delay(500);
                ubtnlab.Foreground = new SolidColorBrush(white);
            }
            else
            {
                ubtnlab.Text = UT.GetLang("udcheck");
            }
        }
    }

    public About(DashboardViewModel viewModel, ISnackbarService snackbarService)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }

    public void Github_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/STY1001/Unowhy-Tools",
                        UseShellExecute = true
        });
    }

    public void STY1001_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://sty1001.cf",
            UseShellExecute = true
        });
    }

    public void Discord_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://discord.com/invite/dw3ZJ9u7WS",
            UseShellExecute = true
        });
    }
}
