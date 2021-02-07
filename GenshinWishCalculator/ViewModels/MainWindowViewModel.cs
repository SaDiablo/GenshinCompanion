using GenshinWishCalculator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenshinWishCalculator.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _UpdateField(ref _duration, value); }
        }

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get { return _startTime; }
            private set { _UpdateField(ref _startTime, value); }
        }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            private set { _UpdateField(ref _remainingTime, value); }
        }

        private bool _running;
        public bool Running
        {
            get { return _running; }
            private set { _UpdateField(ref _running, value, _OnRunningChanged); }
        }

        private Banner _characterBanner = new Banner(Type.Character);
        public Banner CharacterBanner
        {
            get { return _characterBanner; }
            set { _UpdateField(ref _characterBanner, value); }
        }

        private Banner _weaponBanner = new Banner(Type.Weapon);
        public Banner WeaponBanner
        {
            get { return _weaponBanner; }
            set { _UpdateField(ref _weaponBanner, value); }
        }

        private Banner _standardBanner = new Banner(Type.Standard);
        public Banner StandardBanner
        {
            get { return _standardBanner; }
            set { _UpdateField(ref _standardBanner, value); }
        }

        private Banner _noviceBanner = new Banner(Type.Novice);
        public Banner NoviceBanner
        {
            get { return _noviceBanner; }
            set { _UpdateField(ref _noviceBanner, value); }
        }

        private string _inputString;
        public string InputString
        {
            get { return _inputString; }
            set { _UpdateField(ref _inputString, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly DelegateCommand _startCountdownCommand;
        public ICommand StartCountdownCommand { get { return _startCountdownCommand; } }
        
        private readonly DelegateCommand _readInputCommand;
        public ICommand ReadInputCommand { get { return _readInputCommand; } }

        public MainWindowViewModel()
        {
            _startCountdownCommand = new DelegateCommand(_StartCountdown, () => !Running);
            _readInputCommand = new DelegateCommand(_ReadInput);
        }

        private void _OnRunningChanged(bool obj)
        {
            _startCountdownCommand.RaiseCanExecuteChanged();
        }

        private void _UpdateField<T>(ref T field, T newValue,
            Action<T> onChangedCallback = null,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return;
            }

            T oldValue = field;

            field = newValue;
            onChangedCallback?.Invoke(oldValue);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!(object.Equals(field, newValue)))
            {
                field = (newValue);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
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
    }
}
