using GenshinCompanionAvalonia.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GenshinCompanionAvalonia.ViewModels
{
    public class MainWindowViewModel : ModelBase
    {
        #region Fields

        private bool _isVisible;
        public bool IsVisible { get => _isVisible; set => _UpdateField(ref _isVisible, value); }

        private ResinTimer _timer = new ResinTimer();
        public ResinTimer Timer { get => _timer; set => _UpdateField(ref _timer, value); }

        private Banner _characterBanner = new Banner(BannerType.Character);
        public Banner CharacterBanner { get => _characterBanner; set => _UpdateField(ref _characterBanner, value); }

        private Banner _weaponBanner = new Banner(BannerType.Weapon);
        public Banner WeaponBanner { get => _weaponBanner; set => _UpdateField(ref _weaponBanner, value); }

        private Banner _standardBanner = new Banner(BannerType.Standard);
        public Banner StandardBanner { get => _standardBanner; set => _UpdateField(ref _standardBanner, value); }

        private Banner _noviceBanner = new Banner(BannerType.Novice);
        public Banner NoviceBanner { get => _noviceBanner; set => _UpdateField(ref _noviceBanner, value); }

        private string _inputString;
        public string InputString { get => _inputString; set => _UpdateField(ref _inputString, value); }

        private int _tabBannersIndex;
        public int TabBannersIndex { get => _tabBannersIndex; set => _UpdateField(ref _tabBannersIndex, value); }
        #endregion
        #region Delegates
        public ICommand StartCountdownCommand { get; }

        public ICommand AddWishesCommand { get; }

        public ICommand SaveBannersCommand { get; }

        public ICommand OpenBannersCommand { get; }

        public ICommand MinimizeWindowCommand { get; }

        public ICommand EditRemainingTimeCommand { get; }

        public ICommand RemoveWishCommand { get; }
        #endregion

        private void _MinimizeWindow()
        {

        }

        private void _RemoveWish(WishDrop wish)
        {
            switch (TabBannersIndex)
            {
                case 0:
                    CharacterBanner.RemoveItem(wish);
                    break;
                case 1:
                    WeaponBanner.RemoveItem(wish);
                    break;
                case 2:
                    StandardBanner.RemoveItem(wish);
                    break;
                case 3:
                    NoviceBanner.RemoveItem(wish);
                    break;
                default:
                    break;
            }
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
            }
            if (!Timer.Running) { Timer._StartCountdown(); }
            Timer.Save();
        }

        private void _AddWishesCommand()
        {
            switch (TabBannersIndex)
            {
                case 0:
                    CharacterBanner.AddRange(InputString);
                    break;
                case 1:
                    WeaponBanner.AddRange(InputString);
                    break;
                case 2:
                    StandardBanner.AddRange(InputString);
                    break;
                case 3:
                    NoviceBanner.AddRange(InputString);
                    break;
                default:
                    break;
            }
            InputString = string.Empty;
        }

        private void _SaveBanners()
        {
            CharacterBanner.TrySaveJson();
            WeaponBanner.TrySaveJson();
            StandardBanner.TrySaveJson();
            NoviceBanner.TrySaveJson();
        }

        private void _OpenBanners()
        {
            CharacterBanner.Open();
            WeaponBanner.Open();
            StandardBanner.Open();
            NoviceBanner.Open();
        }
        public MainWindowViewModel()
        {
            try
            {
                StartCountdownCommand = ReactiveCommand.Create(Timer._StartCountdown);
                EditRemainingTimeCommand = ReactiveCommand.Create<string>((obj) => _EditRemainingTime(obj));
                AddWishesCommand = ReactiveCommand.Create(_AddWishesCommand);
                SaveBannersCommand = ReactiveCommand.Create(_SaveBanners);
                OpenBannersCommand = ReactiveCommand.Create(_OpenBanners);
                MinimizeWindowCommand = ReactiveCommand.Create(_MinimizeWindow);
                RemoveWishCommand = ReactiveCommand.Create<WishDrop>((wish) => _RemoveWish(wish));
                _OpenBanners();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand BuyMusicCommand { get; }
    }
}
