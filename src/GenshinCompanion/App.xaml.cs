﻿using System.Globalization;
using System.Windows;
using GenshinCompanion.ApplicationUpdater;
using GenshinCompanion.Modules.BannersModule;
using GenshinCompanion.Services;
using GenshinCompanion.Services.Interfaces;
using GenshinCompanion.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using Prism.Modularity;

namespace GenshinCompanion
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
#if RELEASE
            SetUpAppCenter();
#endif
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used in Release configuration")]
        private static void SetUpAppCenter()
        {
            string countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            AppCenter.SetCountryCode(countryCode);
            AppCenter.Configure("34c17fce-3c24-41cb-a48a-a570c781ea25");
            if (AppCenter.Configured)
            {
                AppCenter.Start(typeof(Analytics));
                AppCenter.Start(typeof(Crashes));
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<ApplicationUpdaterService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<BannersModule>();
        }
    }
}