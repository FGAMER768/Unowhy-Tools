﻿using Wpf.Ui.Common.Interfaces;
using Unowhy_Tools_WPF.ViewModels;

using Unowhy_Tools;
using System.Windows;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Unowhy_Tools_WPF.Views.Pages;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class WinRE : INavigableView<DashboardViewModel>
{
    UT.Data UTdata = new UT.Data();

    Color disabled = (Color)ColorConverter.ConvertFromString("#888888");
    Color enabled = (Color)ColorConverter.ConvertFromString("#FFFFFF");

    public DashboardViewModel ViewModel
    {
        get;
    }

    public async void GoBack(object sender, RoutedEventArgs e)
    {
        UT.anim.BackBtnAnim(BackBTN);
        await Task.Delay(150);
        UT.anim.TransitionBack(RootGrid);
        await Task.Delay(200);
        UT.NavigateTo(typeof(Repair));
    }

    public void applylang()
    {
        en_txt.Text = UT.GetLang("enable");
        dis_txt.Text = UT.GetLang("disable");
        rep_txt.Text = UT.GetLang("repair");
    }

    public async Task CheckBTN()
    {
        string out1 = await UT.RunReturn("reagentc.exe", "/info");
        if (out1.Contains("Enable"))
        {
            en.IsEnabled = false;
            en_txt.Foreground = new SolidColorBrush(disabled);
            dis.IsEnabled = true;
            dis_txt.Foreground = new SolidColorBrush(enabled);
            rep.IsEnabled = false;
            rep_txt.Foreground = new SolidColorBrush(disabled);
        }
        else
        {
            if (UTdata.WinRE == true)
            {
                en.IsEnabled = false;
                en_txt.Foreground = new SolidColorBrush(disabled);
                rep.IsEnabled = true;
                rep_txt.Foreground = new SolidColorBrush(enabled);
                dis.IsEnabled = false;
                dis_txt.Foreground = new SolidColorBrush(disabled);
                UT.DialogIShow(UT.GetLang("winremsg"), "no.png");
            }
            else
            {
                en.IsEnabled = true;
                en_txt.Foreground = new SolidColorBrush(enabled);
                rep.IsEnabled = false;
                rep_txt.Foreground = new SolidColorBrush(disabled);
                dis.IsEnabled = false;
                dis_txt.Foreground = new SolidColorBrush(disabled);
            }
        }
    }

    public async void InitAnim(object sender, RoutedEventArgs e)
    {
        foreach (UIElement element in btngrid.Children)
        {
            element.Visibility = Visibility.Hidden;
        }

        UT.anim.BackBtnAnimForw(BackBTN);
        
        await Task.Delay(150);

        foreach (UIElement element in btngrid.Children)
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
                From = 10,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            TranslateTransform transform = new TranslateTransform();
            element.RenderTransform = transform;

            element.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            transform.BeginAnimation(TranslateTransform.YProperty, translateAnimation);

            await Task.Delay(50);
        }
    }

    public async void Enable_Click(object sender, RoutedEventArgs e)
    {
        if (UT.DialogQShow(UT.GetLang("winre.ena"), "enable.png"))
        {
            await UT.waitstatus.open();
            UTdata.WinRE = true;
            await UT.RunMin("reagentc.exe", "/enable");
            await UT.waitstatus.close();
            await CheckBTN();
            if (!en.IsEnabled & !rep.IsEnabled)
            {
                UT.DialogIShow(UT.GetLang("done"), "yes.png");
            }
            else
            {
                UT.DialogIShow(UT.GetLang("failed"), "no.png");
            }
        }
    }

    public async void Disable_Click(object sender, RoutedEventArgs e)
    {
        if(UT.DialogQShow(UT.GetLang("winre.dis"), "disable.png"))
        {
            await UT.waitstatus.open();
            UTdata.WinRE = false;
            await UT.RunMin("reagentc.exe", "/disable");
            await CheckBTN();
            await UT.waitstatus.close();
            if (!dis.IsEnabled)
            {
                UT.DialogIShow(UT.GetLang("done"), "yes.png");
            }
            else
            {
                UT.DialogIShow(UT.GetLang("failed"), "no.png");
            }
        }
    }

    public async void Repair_Click(object sender, RoutedEventArgs e)
    {
        if (UT.CheckInternet())
        {
            await UT.waitstatus.open();
            using (var web = new HttpClient())
            {
                var filebyte = await web.GetByteArrayAsync("https://dl.dropbox.com/s/lahofrvpejlclkx/Winre.wim");
                File.WriteAllBytes("C:\\Windows\\System32\\Recovery\\WinRE.wim", filebyte);
            }

            await UT.RunMin("reagentc.exe", "/setreimage /path C:\\Windows\\System32\\Recovery");
            await UT.RunMin("reagentc.exe", "/enable");

            UTdata.WinRE = true;

            await CheckBTN();
            await UT.waitstatus.close();
            if (dis.IsEnabled)
            {
                UT.DialogIShow(UT.GetLang("done"), "yes.png");
            }
            else
            {
                UT.DialogIShow(UT.GetLang("failed"), "no.png");
            }
        }
        else
        {
            UT.DialogIShow(UT.GetLang("nonet"), "nowifi.png");
        }
    }

    public async void Init(object sender, EventArgs e)
    {
        applylang();
        await CheckBTN();
    }

    public WinRE(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
