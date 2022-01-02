using GenshinCompanion.Modules.BannersModule;
using GenshinCompanion.Services;
using GenshinCompanion.Services.Interfaces;
using GenshinCompanion.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using System.Globalization;
using GenshinCompanion.ApplicationUpdater;

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

        private static void SetUpAppCenter()
        {
            var countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
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