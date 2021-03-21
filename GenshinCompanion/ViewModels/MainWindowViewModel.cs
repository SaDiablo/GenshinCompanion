using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using GenshinWishCalculator.Helpers;
using GenshinWishCalculator.Models;

namespace GenshinWishCalculator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
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
        private readonly DelegateCommand _startCountdownCommand;
        public ICommand StartCountdownCommand => _startCountdownCommand;

        private readonly DelegateCommand _addWishesCommand;
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
        #endregion

        public MainWindowViewModel() 
        {
            try
            {
                _startCountdownCommand = new DelegateCommand(Timer._StartCountdown);
                _editRemainingTimeCommand = new DelegateCommand<string>(_EditRemainingTime);
                _addWishesCommand = new DelegateCommand(_AddWishesCommand);
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
            // Deal with negative numbers/stopping the timer/reseting it
            switch (obj)
            {
                case "-20":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(160);
                    Timer.Save();
                    break;
                case "+20":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(-160);
                    Timer.Save();
                    break;
                default:
                    break;
            }
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
            InputString = String.Empty;
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
    }
}
