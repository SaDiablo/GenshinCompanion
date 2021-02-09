using GenshinWishCalculator.Helpers;
using GenshinWishCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenshinWishCalculator.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged //, IActiveAware
    {
        private bool _running;
        public bool Running { get => _running; set => _UpdateField(ref _running, value, _OnRunningChanged); }

        private ResinTimer _timer = new ResinTimer();
        public ResinTimer Timer { get => _timer; set => _UpdateField(ref _timer, value); }

        private Banner _characterBanner = new Banner(BannerType.Character);
        public Banner CharacterBanner { get => _characterBanner; set { _UpdateField(ref _characterBanner, value); _UpdateField(ref _activeBanner, value); } }

        private Banner _weaponBanner = new Banner(BannerType.Weapon);
        public Banner WeaponBanner { get => _weaponBanner; set { _UpdateField(ref _weaponBanner, value); _UpdateField(ref _activeBanner, value); } }

        private Banner _standardBanner = new Banner(BannerType.Standard);
        public Banner StandardBanner { get => _standardBanner; set { _UpdateField(ref _standardBanner, value); _UpdateField(ref _activeBanner, value); } }

        private Banner _noviceBanner = new Banner(BannerType.Novice);
        public Banner NoviceBanner { get => _noviceBanner; set { _UpdateField(ref _noviceBanner, value); _UpdateField(ref _activeBanner, value); }  }

        private Banner _activeBanner;
        public Banner ActiveBanner 
        { 
            get
            {
                switch (TabBannersIndex)
                {
                    case 0:
                        return CharacterBanner;
                    case 1:
                        return WeaponBanner;
                    case 2:
                        return StandardBanner;
                    case 3:
                        return NoviceBanner;
                    default:
                        break;
                }
                return _activeBanner;
            } 
        }

        private string _inputString;
        public string InputString { get => _inputString; set => _UpdateField(ref _inputString, value); }

        private int _tabBannersIndex;
        public int TabBannersIndex { get => _tabBannersIndex; set => _UpdateField(ref _tabBannersIndex, value); }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly DelegateCommand _startCountdownCommand;
        public ICommand StartCountdownCommand => _startCountdownCommand;

        private readonly DelegateCommand _addWishesCommand;
        public ICommand AddWishesCommand => _addWishesCommand;

        private readonly DelegateCommand _updateNumbersCommand;
        public ICommand UpdateNumbersCommand => _updateNumbersCommand;

        private readonly DelegateCommand _saveBannersCommand;
        public ICommand SaveBannersCommand => _saveBannersCommand;

        private readonly DelegateCommand _openBannersCommand;
        public ICommand OpenBannersCommand => _openBannersCommand;

        public MainWindowViewModel() 
        {
            _startCountdownCommand = new DelegateCommand(Timer._StartCountdown, () => !Running);
            _addWishesCommand = new DelegateCommand(_AddWishesCommand);
            _updateNumbersCommand = new DelegateCommand(_UpdateNumbers);
            _saveBannersCommand = new DelegateCommand(_SaveBanners);
            _openBannersCommand = new DelegateCommand(_OpenBanners);
        }

        private void _OnRunningChanged(bool obj) => _startCountdownCommand.RaiseCanExecuteChanged();

        private bool _UpdateField<T>(ref T field, T newValue, Action<T> onChangedCallback = null,
                                     [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, newValue))
            {
                return false;
            }

            T oldValue = field;

            field = newValue;
            onChangedCallback?.Invoke(oldValue);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
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

        private void _UpdateNumbers()
        {
            
        }

        private void _SaveBanners()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            if (CharacterBanner != null & CharacterBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("characterBanner.json"))
                {
                    var json = JsonSerializer.Serialize(CharacterBanner, options);
                    file.Write(json);
                }
            }
            if (WeaponBanner != null & WeaponBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("weaponBanner.json"))
                {
                    var json = JsonSerializer.Serialize(WeaponBanner, options);
                    file.Write(json);
                }
            }
            if (StandardBanner != null & StandardBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("standardBanner.json"))
                {
                    var json = JsonSerializer.Serialize(StandardBanner, options);
                    file.Write(json);
                }
            }
            if (NoviceBanner != null & NoviceBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("noviceBanner.json"))
                {
                    var json = JsonSerializer.Serialize(NoviceBanner, options);
                    file.Write(json);
                }
            }
        }

        private async void _OpenBanners()
        {
            try
            {
                using (FileStream openStream = File.OpenRead("characterBanner.json"))
                {
                    CharacterBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
            try
            {
                using (FileStream openStream = File.OpenRead("weaponBanner.json"))
                {
                    WeaponBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
            try
            {
                using (FileStream openStream = File.OpenRead("standardBanner.json"))
                {
                    StandardBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
            try
            {
                using (FileStream openStream = File.OpenRead("noviceBanner.json"))
                {
                    NoviceBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
        }
    }
}
