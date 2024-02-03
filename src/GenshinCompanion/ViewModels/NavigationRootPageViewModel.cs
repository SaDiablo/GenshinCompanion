using System.Windows.Input;
using GenshinCompanion.Core;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace GenshinCompanion.ViewModels
{
    public class NavigationRootPageViewModel : BindableBase
    {
        private string _title = "Genshin Companion";
        private readonly IRegionManager regionManager;
        private readonly DelegateCommand<NavigationViewItemInvokedEventArgs> navigationViewItemInvokedCommand;
        public ICommand NavigationViewItemInvokedCommand => navigationViewItemInvokedCommand;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public NavigationRootPageViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            navigationViewItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(NavView_Navigate);
        }

        private void NavView_Navigate(NavigationViewItemInvokedEventArgs item)
        {
            switch (item.InvokedItemContainer.Tag)
            {
                case "CharacterBannerTag":
                    regionManager.RequestNavigate(RegionNames.BannersRegion, "CharacterBannerView");
                    break;

                case "WeaponBannerTag":
                    regionManager.RequestNavigate(RegionNames.BannersRegion, "WeaponBannerView");
                    break;

                case "StandardBannerTag":
                    regionManager.RequestNavigate(RegionNames.BannersRegion, "StandardBannerView");
                    break;

                case "NoviceBannerTag":
                    regionManager.RequestNavigate(RegionNames.BannersRegion, "NoviceBannerView");
                    break;

                case "TimersTag":
                    regionManager.RequestNavigate(RegionNames.BannersRegion, "TimersView");
                    break;
            }
        }
    }
}