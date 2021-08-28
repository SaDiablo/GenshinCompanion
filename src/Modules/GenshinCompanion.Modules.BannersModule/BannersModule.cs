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
        private readonly IRegionManager _regionManager;

        public BannersModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager
                .RegisterViewWithRegion(RegionNames.BannersRegion, "CharacterBannerView")
                .RegisterViewWithRegion(RegionNames.BannersRegion, "WeaponBannerView")
                .RegisterViewWithRegion(RegionNames.BannersRegion, "StandardBannerView")
                .RegisterViewWithRegion(RegionNames.BannersRegion, "NoviceBannerView");
            _regionManager.RequestNavigate(RegionNames.BannersRegion, "CharacterBannerView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CharacterBannerView>();
            containerRegistry.RegisterForNavigation<WeaponBannerView>();
            containerRegistry.RegisterForNavigation<StandardBannerView>();
            containerRegistry.RegisterForNavigation<NoviceBannerView>();
            ViewModelLocationProvider.Register<CharacterBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<WeaponBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<StandardBannerView, BannersViewModel>();
            ViewModelLocationProvider.Register<NoviceBannerView, BannersViewModel>();
        }
    }
}