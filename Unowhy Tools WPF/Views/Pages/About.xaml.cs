﻿using Wpf.Ui.Common.Interfaces;
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
using System.Xml.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

    public void GoForw(object sender, RoutedEventArgs e)
    {
        UT.anim.TransitionForw(RootGrid);
    }

    public async void GoBack(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animsb = new DoubleAnimation();
        animsb.From = 20;
        animsb.To = 0;
        animsb.Duration = TimeSpan.FromMilliseconds(500);
        animsb.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        DoubleAnimation animsb2 = new DoubleAnimation();
        animsb2.From = -20;
        animsb2.To = 0;
        animsb2.Duration = TimeSpan.FromMilliseconds(500);
        animsb2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform transsb = new TranslateTransform();
        TranslateTransform transsb2 = new TranslateTransform();
        SB1.RenderTransform = transsb;
        SB2.RenderTransform = transsb2;
        transsb.BeginAnimation(TranslateTransform.XProperty, animsb);
        transsb.BeginAnimation(TranslateTransform.YProperty, animsb2);
        transsb2.BeginAnimation(TranslateTransform.XProperty, animsb2);
        transsb2.BeginAnimation(TranslateTransform.YProperty, animsb);

        DoubleAnimation animqo = new DoubleAnimation();
        animqo.From = 0;
        animqo.To = 250;
        animqo.Duration = TimeSpan.FromMilliseconds(500);
        animqo.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform transqo = new TranslateTransform();
        OpGrid.RenderTransform = transqo;
        transqo.BeginAnimation(TranslateTransform.YProperty, animqo);

        DoubleAnimation animlogo = new DoubleAnimation();
        animlogo.From = 0;
        animlogo.To = -40;
        animlogo.Duration = TimeSpan.FromMilliseconds(500);
        animlogo.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        DoubleAnimation animlogo2 = new DoubleAnimation();
        animlogo2.From = 0;
        animlogo2.To = 187;
        animlogo2.Duration = TimeSpan.FromMilliseconds(500);
        animlogo2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform translogo = new TranslateTransform();
        LogoGrid.RenderTransform = translogo;
        translogo.BeginAnimation(TranslateTransform.YProperty, animlogo);
        translogo.BeginAnimation(TranslateTransform.XProperty, animlogo2);

        foreach (UIElement element in InfoStack.Children)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation translateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 150,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            TranslateTransform transform = new TranslateTransform();
            element.RenderTransform = transform;

            element.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            transform.BeginAnimation(TranslateTransform.XProperty, translateAnimation);

            await Task.Delay(10);
        }

        UT.anim.BackBtnAnim(BackBTN);

        await Task.Delay(500);

        var mainWindow = System.Windows.Application.Current.MainWindow as Unowhy_Tools_WPF.Views.MainWindow;
        mainWindow.RootNavigation.TransitionType = Wpf.Ui.Animations.TransitionType.None;
        UT.NavigateTo(typeof(Dashboard));
        await Task.Delay(1000);
        mainWindow.RootNavigation.TransitionType = Wpf.Ui.Animations.TransitionType.SlideBottom;
    }

    public void applylang()
    {
        ubtnlab.Text = UT.GetLang("udcheck");
    }

    public async void Init(object sender, EventArgs e)
    {
        applylang();
        string UTsver = "Unowhy Tools version " + UT.version.getverfull().ToString().Insert(2, ".") + " (Build " + UT.version.getverbuild().ToString() + ") ";

        if (UT.version.isdeb()) UTsver = UTsver + "(Debug)";
        else UTsver = UTsver + "(Release)";

        string UTSsver = "Unowhy Tools Service version " + await UT.UTS.UTSmsg("UTS", "GetVer");

        LogoVer.Text = UTsver;
        UTverlab.Text = UTsver;
        UTSverlab.Text = UTSsver;

        if (UT.CheckInternet())
        {
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

                    while (true)
                    {
                        ubtnlab.Foreground = new SolidColorBrush(white);
                        await Task.Delay(500);
                        ubtnlab.Foreground = new SolidColorBrush(gray);
                        await Task.Delay(500);
                    }
                }
                else
                {
                    ubtnlab.Text = UT.GetLang("udcheck");
                }
            }
        }
        else
        {
            ubtnlab.Text = UT.GetLang("udcheck");
        }
    }

    public async void InitAnim(object sender, System.Windows.RoutedEventArgs e)
    {
        UT.anim.BackBtnAnimForw(BackBTN);

        DoubleAnimation animis = new DoubleAnimation();
        animis.From = 0;
        animis.To = 0;
        animis.Duration = TimeSpan.FromMilliseconds(0);
        animis.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform transis = new TranslateTransform();
        InfoStack.RenderTransform = transis;
        transis.BeginAnimation(TranslateTransform.YProperty, animis);

        DoubleAnimation animqo = new DoubleAnimation();
        animqo.From = 0;
        animqo.To = 0;
        animqo.Duration = TimeSpan.FromMilliseconds(0);
        animqo.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform transqo = new TranslateTransform();
        OpGrid.RenderTransform = transqo;
        transqo.BeginAnimation(TranslateTransform.YProperty, animqo);

        DoubleAnimation animlogo = new DoubleAnimation();
        animlogo.From = 0;
        animlogo.To = 0;
        animlogo.Duration = TimeSpan.FromMilliseconds(0);
        animlogo.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        DoubleAnimation animlogo2 = new DoubleAnimation();
        animlogo2.From = 0;
        animlogo2.To = 0;
        animlogo2.Duration = TimeSpan.FromMilliseconds(0);
        animlogo2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform translogo = new TranslateTransform();
        LogoGrid.RenderTransform = translogo;
        translogo.BeginAnimation(TranslateTransform.YProperty, animlogo);
        translogo.BeginAnimation(TranslateTransform.XProperty, animlogo2);

        DoubleAnimation preanimsb = new DoubleAnimation();
        preanimsb.From = 0;
        preanimsb.To = 0;
        preanimsb.Duration = TimeSpan.FromMilliseconds(0);
        preanimsb.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        DoubleAnimation preanimsb2 = new DoubleAnimation();
        preanimsb2.From = 0;
        preanimsb2.To = 0;
        preanimsb2.Duration = TimeSpan.FromMilliseconds(0);
        preanimsb2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform pretranssb = new TranslateTransform();
        TranslateTransform pretranssb2 = new TranslateTransform();
        SB1.RenderTransform = pretranssb;
        SB2.RenderTransform = pretranssb2;
        pretranssb.BeginAnimation(TranslateTransform.XProperty, preanimsb);
        pretranssb.BeginAnimation(TranslateTransform.YProperty, preanimsb2);
        pretranssb2.BeginAnimation(TranslateTransform.XProperty, preanimsb2);
        pretranssb2.BeginAnimation(TranslateTransform.YProperty, preanimsb);

        DoubleAnimation animsb = new DoubleAnimation();
        animsb.From = 0;
        animsb.To = 20;
        animsb.Duration = TimeSpan.FromMilliseconds(1800);
        animsb.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        DoubleAnimation animsb2 = new DoubleAnimation();
        animsb2.From = 0;
        animsb2.To = -20;
        animsb2.Duration = TimeSpan.FromMilliseconds(1800);
        animsb2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 5 };
        TranslateTransform transsb = new TranslateTransform();
        TranslateTransform transsb2 = new TranslateTransform();
        SB1.RenderTransform = transsb;
        SB2.RenderTransform = transsb2;
        transsb.BeginAnimation(TranslateTransform.XProperty, animsb);
        transsb.BeginAnimation(TranslateTransform.YProperty, animsb2);
        transsb2.BeginAnimation(TranslateTransform.XProperty, animsb2);
        transsb2.BeginAnimation(TranslateTransform.YProperty, animsb);

        foreach (UIElement element in InfoStack.Children)
        {
            element.Visibility = Visibility.Hidden;
        }

        foreach (UIElement element in OpGrid.Children)
        {
            element.Visibility = Visibility.Hidden;
        }

        foreach (UIElement element in InfoStack.Children)
        {
            element.Visibility = Visibility.Visible;
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation translateAnimation = new DoubleAnimation
            {
                From = 150,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            TranslateTransform transform = new TranslateTransform();
            element.RenderTransform = transform;

            element.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            transform.BeginAnimation(TranslateTransform.XProperty, translateAnimation);

            await Task.Delay(50);
        }

        foreach (UIElement element in OpGrid.Children)
        {
            element.Visibility = Visibility.Visible;
            DoubleAnimation translateAnimation = new DoubleAnimation
            {
                From = 150,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3}
            };

            TranslateTransform transform = new TranslateTransform();
            element.RenderTransform = transform;

            transform.BeginAnimation(TranslateTransform.YProperty, translateAnimation);

            await Task.Delay(100);
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
            FileName = "https://sty1001.fr",
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
