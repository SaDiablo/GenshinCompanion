using GenshinCompanion.CoreStandard;
using GenshinCompanion.Services;
using Prism.Mvvm;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenshinCompanion.Modules.BannersModule.Models
{
    public class ResinTimer : BindableBase, IPersistData
    {
        private TimeSpan duration;

        public TimeSpan Duration
        {
            get => duration;
            set
            {
                EndTime = DateTime.UtcNow + value;
                Save();
                SetProperty(ref duration, new TimeSpan(0, 0, 0));
                if (!running) _StartCountdown();
            }
        }

        private DateTime? startTime;
        public DateTime? StartTime { get => startTime; set => SetProperty(ref startTime, value); }

        private TimeSpan remainingTime;
        public TimeSpan RemainingTime { get => remainingTime; set => SetProperty(ref remainingTime, value); }

        private DateTime? endTime;

        public DateTime? EndTime
        {
            get => endTime;
            set
            {
                if (SetProperty(ref endTime, value))
                {
                    EndTime120 = value.Value.AddMinutes(-320);
                    EndTime80 = value.Value.AddMinutes(-640);
                    EndTime40 = value.Value.AddMinutes(-960);
                    EndTime20 = value.Value.AddMinutes(-1120);
                    Save();
                }
            }
        }

        private DateTime? endTime120;

        public DateTime? EndTime120
        {
            get => endTime120;
            set => SetProperty(ref endTime120, value);
        }
        private DateTime? endTime80;

        public DateTime? EndTime80
        {
            get => endTime80;
            set => SetProperty(ref endTime80, value);
        }
        private DateTime? endTime40;

        public DateTime? EndTime40
        {
            get => endTime40;
            set => SetProperty(ref endTime40, value);
        }
        private DateTime? endTime20;

        public DateTime? EndTime20
        {
            get => endTime20;
            set => SetProperty(ref endTime20, value);
        }

        private bool running;

        public bool Running { get => running; set => SetProperty(ref running, value); }

        public ResinTimer()
        {
            Open();
        }

        public async void _StartCountdown()
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

        public async void Open()
        {
            object deserializedData = await DataProvider.Open<DateTime?>(nameof(ResinTimer), DataFolder.Settings);

            if (deserializedData != null)
            {
                EndTime = (DateTime?)deserializedData;
                RaisePropertyChanged("EndTime");
                _StartCountdown();
            }
        }

        public void Save()
        {
            if (EndTime != null & EndTime.HasValue)
            {
                DataProvider.Save(EndTime, nameof(ResinTimer), DataFolder.Settings);
            }
        }
    }
}