using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GenshinWishCalculator.Models
{
    public class ResinTimer : INotifyPropertyChanged
    {
        private TimeSpan _duration;
        public TimeSpan Duration 
        { 
            get => _duration; 
            set 
            {
                EndTime = DateTime.UtcNow + value;
                _UpdateField(ref _duration, new TimeSpan(0,0,0));
            }
        }

        private DateTime? _startTime;
        public DateTime? StartTime { get => _startTime; set => _UpdateField(ref _startTime, value); }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime { get => _remainingTime; set => _UpdateField(ref _remainingTime, value); }

        private DateTime? _endTime;
        public DateTime? EndTime { get => _endTime; set => _UpdateField(ref _endTime, value); }

        private bool _running;
        public bool Running { get => _running; set => _running = value; }

        public event PropertyChangedEventHandler PropertyChanged;
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
        

        internal async void _StartCountdown()
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

            DateTime startTime = DateTime.UtcNow; 
            //EndTime = startTime + Duration;
            TimeSpan remainingTime, interval = TimeSpan.FromMilliseconds(500);

            StartTime = startTime;

            remainingTime = (EndTime - startTime) ?? TimeSpan.Zero;

            while (remainingTime > TimeSpan.Zero)
            {
                RemainingTime = remainingTime;
                if (RemainingTime < interval)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Resin"));
                    interval = RemainingTime;
                }

                // NOTE: arbitrary update rate of 100 ms (initialized above). This
                // should be a value at least somewhat less than the minimum precision
                // displayed (e.g. here it's 1/10th the displayed precision of one
                // second), to avoid potentially distracting/annoying "stutters" in
                // the countdown.
                
                await Task.Delay(interval);
                remainingTime = EndTime.Value - DateTime.UtcNow;
            }

            RemainingTime = TimeSpan.Zero;
            StartTime = null;
            Running = false;
        }
    }
}
