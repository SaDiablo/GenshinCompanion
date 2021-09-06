using GenshinCompanion.Core.Mvvm;
using GenshinCompanion.Modules.BannersModule.Models;
using GenshinCompanion.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace GenshinCompanion.Modules.BannersModule.ViewModels
{
    public class BannersViewModel : RegionViewModelBase
    {
        public BannersViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            try
            {
                _startCountdownCommand = new DelegateCommand(Timer._StartCountdown);
                _editRemainingTimeCommand = new DelegateCommand<string>(_EditRemainingTime);
                _addWishesCommand = new DelegateCommand<string>(_AddWishesCommand);
                _saveBannersCommand = new DelegateCommand(_SaveBanners);
                _openBannersCommand = new DelegateCommand(_OpenBanners);
                _minimizeWindowCommand = new DelegateCommand(_MinimizeWindow);
                _removeWishCommand = new DelegateCommand<WishDrop>(_RemoveWish);
                _OpenBanners();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private bool _isVisible;
        public bool IsVisible { get => _isVisible; set => SetProperty(ref _isVisible, value); }

        private ResinTimer _timer = new ResinTimer();
        public ResinTimer Timer { get => _timer; set => SetProperty(ref _timer, value); }

        private Banner _characterBanner = new Banner(BannerType.Character);
        public Banner CharacterBanner { get => _characterBanner; set => SetProperty(ref _characterBanner, value); }

        private Banner _weaponBanner = new Banner(BannerType.Weapon);
        public Banner WeaponBanner { get => _weaponBanner; set => SetProperty(ref _weaponBanner, value); }

        private Banner _standardBanner = new Banner(BannerType.Standard);
        public Banner StandardBanner { get => _standardBanner; set => SetProperty(ref _standardBanner, value); }

        private Banner _noviceBanner = new Banner(BannerType.Novice);
        public Banner NoviceBanner { get => _noviceBanner; set => SetProperty(ref _noviceBanner, value); }

        private string _inputString;
        public string InputString { get => _inputString; set => SetProperty(ref _inputString, value); }

        private int _tabBannersIndex;
        public int TabBannersIndex { get => _tabBannersIndex; set => SetProperty(ref _tabBannersIndex, value); }

        private readonly DelegateCommand _startCountdownCommand;
        public ICommand StartCountdownCommand => _startCountdownCommand;

        private readonly DelegateCommand<string> _addWishesCommand;
        public ICommand AddWishesCommand => _addWishesCommand;

        private readonly DelegateCommand _saveBannersCommand;
        public ICommand SaveBannersCommand => _saveBannersCommand;

        private readonly DelegateCommand _openBannersCommand;
        public ICommand OpenBannersCommand => _openBannersCommand;

        private readonly DelegateCommand _minimizeWindowCommand;
        public ICommand MinimizeWindowCommand => _minimizeWindowCommand;

        private readonly DelegateCommand<string> _editRemainingTimeCommand;
        public ICommand EditRemainingTimeCommand => _editRemainingTimeCommand;

        private readonly DelegateCommand<WishDrop> _removeWishCommand;
        public ICommand RemoveWishCommand => _removeWishCommand;

        private void _MinimizeWindow()
        {
        }

        private void _RemoveWish(WishDrop wish)
        {
            CharacterBanner.WishList.Remove(wish);
            WeaponBanner.WishList.Remove(wish);
            StandardBanner.WishList.Remove(wish);
            NoviceBanner.WishList.Remove(wish);
            _SaveBanners();
        }

        private void _EditRemainingTime(string obj)
        {
            if (Timer.EndTime is null || Timer.EndTime.Value < DateTime.UtcNow)
            {
                Timer.EndTime = DateTime.UtcNow;
            }

            // Deal with negative numbers/stopping the timer/reseting it
            switch (obj)
            {
                case "-20":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(160);
                    break;
                case "+20":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(-160);
                    break;
                case "-10":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(80);
                    break;
                case "+10":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(-80);
                    break;
            }

            if (!Timer.Running)
            {
                Timer._StartCountdown();
            }

            Timer.Save();
        }

        private void _AddWishesCommand(string bannerType)
        {
            switch (bannerType)
            {
                case "Character":
                    CharacterBanner.AddRange(InputString);
                    break;

                case "Weapon":
                    WeaponBanner.AddRange(InputString);
                    break;

                case "Standard":
                    StandardBanner.AddRange(InputString);
                    break;

                case "Novice":
                    NoviceBanner.AddRange(InputString);
                    break;

                default:
                    break;
            }
            InputString = string.Empty;
            _SaveBanners();
        }

        private void _SaveBanners()
        {
            CharacterBanner.Save();
            WeaponBanner.Save();
            StandardBanner.Save();
            NoviceBanner.Save();
        }

        private void _OpenBanners()
        {
            CharacterBanner.Open();
            WeaponBanner.Open();
            StandardBanner.Open();
            NoviceBanner.Open();
        }
    }
}