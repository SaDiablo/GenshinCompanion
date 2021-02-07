using GenshinWishCalculator.Helpers;
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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private TimeSpan _duration;
        public TimeSpan Duration { get => _duration; set => _UpdateField(ref _duration, value); }

        private DateTime? _startTime;
        public DateTime? StartTime { get => _startTime; set => _UpdateField(ref _startTime, value); }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime { get => _remainingTime; set => _UpdateField(ref _remainingTime, value); }

        private bool _running;
        public bool Running { get => _running; set => _UpdateField(ref _running, value, _OnRunningChanged); }

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

        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly DelegateCommand _startCountdownCommand;
        public ICommand StartCountdownCommand => _startCountdownCommand;

        private readonly DelegateCommand _readInputCommand;
        public ICommand ReadInputCommand => _readInputCommand;

        private readonly DelegateCommand _updateNumbersCommand;
        public ICommand UpdateNumbersCommand => _updateNumbersCommand;

        private readonly DelegateCommand _saveBannersCommand;
        public ICommand SaveBannersCommand => _saveBannersCommand;

        public MainWindowViewModel() 
        {
            _startCountdownCommand = new DelegateCommand(_StartCountdown, () => !Running);
            _readInputCommand = new DelegateCommand(_ReadInput);
            _updateNumbersCommand = new DelegateCommand(_UpdateNumbers);
            _saveBannersCommand = new DelegateCommand(_SaveBanners);
        }

        private void _OnRunningChanged(bool obj) => _startCountdownCommand.RaiseCanExecuteChanged();

        private bool _UpdateField<T>(ref T field, T newValue,
            Action<T> onChangedCallback = null,
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

        private async void _StartCountdown()
        {
            Running = true;

            // NOTE: UTC times used internally to ensure proper operation
            // across Daylight Saving Time changes. An IValueConverter can
            // be used to present the user a local time.

            // NOTE: RemainingTime is the raw data. It may be desirable to
            // use an IValueConverter to always round up to the nearest integer
            // value for whatever is the least-significant component displayed
            // (e.g. minutes, seconds, milliseconds), so that the displayed
            // value doesn't reach the zero value until the timer has completed.

            DateTime startTime = DateTime.UtcNow, endTime = startTime + Duration;
            TimeSpan remainingTime, interval = TimeSpan.FromMilliseconds(100);

            StartTime = startTime;
            remainingTime = endTime - startTime;

            while (remainingTime > TimeSpan.Zero)
            {
                RemainingTime = remainingTime;
                if (RemainingTime < interval)
                {
                    interval = RemainingTime;
                }

                // NOTE: arbitrary update rate of 100 ms (initialized above). This
                // should be a value at least somewhat less than the minimum precision
                // displayed (e.g. here it's 1/10th the displayed precision of one
                // second), to avoid potentially distracting/annoying "stutters" in
                // the countdown.

                await Task.Delay(interval);
                remainingTime = endTime - DateTime.UtcNow;
            }

            RemainingTime = TimeSpan.Zero;
            StartTime = null;
            Running = false;
        }

        private async void _ReadInput()
        {
            Running = true;

            // NOTE: UTC times used internally to ensure proper operation
            // across Daylight Saving Time changes. An IValueConverter can
            // be used to present the user a local time.

            // NOTE: RemainingTime is the raw data. It may be desirable to
            // use an IValueConverter to always round up to the nearest integer
            // value for whatever is the least-significant component displayed
            // (e.g. minutes, seconds, milliseconds), so that the displayed
            // value doesn't reach the zero value until the timer has completed.

            DateTime startTime = DateTime.UtcNow, endTime = startTime + Duration;
            TimeSpan remainingTime, interval = TimeSpan.FromMilliseconds(100);

            StartTime = startTime;
            remainingTime = endTime - startTime;



            while (remainingTime > TimeSpan.Zero)
            {
                RemainingTime = remainingTime;
                if (RemainingTime < interval)
                {
                    interval = RemainingTime;
                }

                // NOTE: arbitrary update rate of 100 ms (initialized above). This
                // should be a value at least somewhat less than the minimum precision
                // displayed (e.g. here it's 1/10th the displayed precision of one
                // second), to avoid potentially distracting/annoying "stutters" in
                // the countdown.

                await Task.Delay(interval);
                remainingTime = endTime - DateTime.UtcNow;
            }

            RemainingTime = TimeSpan.Zero;
            StartTime = null;
            Running = false;
        }

        private void _UpdateNumbers()
        {
            //if (activeBanner != null & activeBanner.WishList.Count > 0)
            //{
            //    //todo
            //    int wishCount = activeBanner.WishList.Count;
            //    int characterCount = activeBanner.WishList.Where(s => s != null && s.DropType.Equals(DropType.Character)).Count();
            //    int fourStarCount = activeBanner.WishList.Where(s => s != null && s.DropRarity.Equals(4)).Count();
            //    int fiveStarCount = activeBanner.WishList.Where(s => s != null && s.DropRarity.Equals(5)).Count();
            //    //check if exists
            //    //int wishesTill5Star = activeBanner.WishList.FindIndex(s => s.DropRarity.Equals(5)) + 1;
            //    int wishesTill5Star = activeBanner.WishList.Count;
            //    try
            //    {
            //        wishesTill5Star = activeBanner.WishList.Select((wish, index) => new { wish, index }).Where(s => s != null && s.wish.DropRarity.Equals(5)).First().index + 1;
            //    }
            //    catch (InvalidOperationException) { }

            //    double characterPercentCount = (double)characterCount / (double)wishCount;
            //    double fourStarPercentCount = (double)fourStarCount / (double)wishCount;
            //    double fiveStarPercentCount = (double)fiveStarCount / (double)wishCount;

            //    labelWishCount.Content = wishCount;
            //    labelPrimogemsCount.Content = wishCount * 160;
            //    labelCharacterCount.Content = characterCount;
            //    labelCharacterPercentCount.Content = characterPercentCount.ToString("P2");
            //    label4StarCount.Content = fourStarCount;
            //    label4StarPercentCount.Content = fourStarPercentCount.ToString("P2");
            //    label5StarCount.Content = fiveStarCount;
            //    label5StarPercentCount.Content = fiveStarPercentCount.ToString("P2");
            //    labelwishesTillPityCount.Content = activeBanner.PityLimit - wishesTill5Star;
            //    labelwishesTillPityPrimogemsCount.Content = (activeBanner.PityLimit - wishesTill5Star) * 160;
            //}
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
    }
}
