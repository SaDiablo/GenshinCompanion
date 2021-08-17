using GenshinCompanion.Core;
using GenshinCompanion.Modules.BannersModule.Views;
using Prism.Ioc;
using Prism.Modularity;
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
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "ViewA");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
        }
    }
}