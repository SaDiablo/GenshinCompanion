using GenshinCompanion.Core;
using GenshinCompanion.Modules.BannersModule.ViewModels;
using GenshinCompanion.Modules.BannersModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace GenshinCompanion.Modules.BannersModule
{
    public class BannersModule : IModule
    {
        private readonly IRegionManager regionManager;

        public BannersModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager
                .RegisterViewWithRegion(RegionNames.BannersRegion, nameof(CharacterBannerView))
                .RegisterViewWithRegion(RegionNames.BannersRegion, nameof(WeaponBannerView))
                .RegisterViewWithRegion(RegionNames.BannersRegion, nameof(StandardBannerView))
                .RegisterViewWithRegion(RegionNames.BannersRegion, nameof(NoviceBannerView))
                .RegisterViewWithRegion(RegionNames.BannersRegion, nameof(TimersView));
            regionManager.RequestNavigate(RegionNames.BannersRegion, nameof(CharacterBannerView));

            regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, nameof(StatusBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<BannersViewModel>();
            containerRegistry.RegisterSingleton<StatusBarViewModel>();

            containerRegistry.RegisterForNavigation<CharacterBannerView>();
            containerRegistry.RegisterForNavigation<WeaponBannerView>();
            containerRegistry.RegisterForNavigation<StandardBannerView>();
            containerRegistry.RegisterForNavigation<NoviceBannerView>();
            containerRegistry.RegisterForNavigation<TimersView>();
            containerRegistry.RegisterForNavigation<StatusBarView>();

            ViewModelLocationProvider.Register<CharacterBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<WeaponBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<StandardBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<NoviceBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<TimersView, TimersViewModel>();
            ViewModelLocationProvider.Register<StatusBarView, StatusBarViewModel>();
        }
    }
}