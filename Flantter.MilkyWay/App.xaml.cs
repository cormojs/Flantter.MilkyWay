﻿using Flantter.MilkyWay.Setting;
using Prism.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// 空のアプリケーション テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234227 を参照してください

namespace Flantter.MilkyWay
{
    sealed partial class App : PrismApplication
    {
        public App()
        {
            this.InitializeComponent();

            this.UnhandledException += App_UnhandledException;
            
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(Microsoft.ApplicationInsights.WindowsCollectors.Metadata | Microsoft.ApplicationInsights.WindowsCollectors.Session);
        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
#if _DEBUG
            // デバッグ用コードをここに突っ込む
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Models.Filter.Compiler.Compile("(!(User.ScreenName In [\"cucmberium\", \"cucmberium_sub\"] || User.Id !In [10, 20, 30]) && RetweetCount >= FavoriteCount * 10 + 10 / (2 + 3))");
            sw.Stop();
            System.Diagnostics.Debug.WriteLine(sw.Elapsed);
#endif

            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 320, Height = 500 });

            try
            {
                AdvancedSettingService.AdvancedSetting.LoadFromAppSettings();
            }
            catch
            {
            }

            if (AdvancedSettingService.AdvancedSetting.Account == null || AdvancedSettingService.AdvancedSetting.Account.Count == 0)
                this.NavigationService.Navigate("Initialize", args.Arguments);
            else
                this.NavigationService.Navigate("Main", args.Arguments);

            return Task.FromResult<object>(null);
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            return base.OnInitializeAsync(args);
        }
    }
}