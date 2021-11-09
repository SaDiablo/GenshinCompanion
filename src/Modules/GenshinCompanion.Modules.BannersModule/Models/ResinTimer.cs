using GenshinCompanion.CoreStandard;
using GenshinCompanion.Services;
using Microsoft.AppCenter.Analytics;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

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
                TimerService.EndTime = DateTime.UtcNow + value;
                Save();
                SetProperty(ref duration, new TimeSpan(0, 0, 0));
                if (!Running) StartCountdown();
            }
        }

        private DateTime? startTime;
        public DateTime? StartTime { get => startTime; set => SetProperty(ref startTime, value); }

        public TimeSpan RemainingTime => TimerService != null ? TimerService.RemainingTime : TimeSpan.Zero;

        private DateTime? endTime;

        public DateTime? EndTime
        {
            get => endTime;
            set
            {
                if (SetProperty(ref endTime, value))
                {
                    if (TimerService != null)
                    {
                        TimerService.EndTime = value;
                    }
                    EndTime120 = value.Value.AddMinutes(-320);
                    EndTime80 = value.Value.AddMinutes(-640);
                    EndTime40 = value.Value.AddMinutes(-960);
                    EndTime20 = value.Value.AddMinutes(-1120);
                    Save();
                }
            }
        }

        public TimerService TimerService { get => timerService; set => SetProperty(ref timerService, value); }

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
        private TimerService timerService;

        public bool Running { get => TimerService.GetRunning(); }

        public ResinTimer()
        {
            Open();
        }

        public void StartCountdown()
        {
            Analytics.TrackEvent("Timer", new Dictionary<string, string> { { "Action", "Started" } });
            TimerService.SetRunning(true);
        }

        public async void Open()
        {
            object deserializedData = await DataProvider.Open<DateTime?>(nameof(ResinTimer), DataFolder.Settings);

            if (deserializedData != null)
            {
                EndTime = (DateTime?)deserializedData;
                RaisePropertyChanged(nameof(EndTime));
                TimerService = new TimerService(EndTime);
            }
        }

        public void Save()
        {
            if (TimerService != null && TimerService.EndTime != null && TimerService.EndTime.HasValue)
            {
                DataProvider.Save(TimerService.EndTime, nameof(ResinTimer), DataFolder.Settings);
            }
        }
    }
}