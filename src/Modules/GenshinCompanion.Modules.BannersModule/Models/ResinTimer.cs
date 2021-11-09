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
                if (!Running)
                {
                    StartCountdown();
                }
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
                    Save();
                }
            }
        }

        public TimerService TimerService { get => timerService; set => SetProperty(ref timerService, value); }

        public DateTime? EndTime120 => TimerService.EndTime.Value.AddMinutes(-320);

        public DateTime? EndTime80 => TimerService.EndTime.Value.AddMinutes(-640);

        public DateTime? EndTime40 => TimerService.EndTime.Value.AddMinutes(-960);

        public DateTime? EndTime20 => TimerService.EndTime.Value.AddMinutes(-1120);

        public bool Running => TimerService.GetRunning();

        private TimerService timerService;

        public ResinTimer() => Open();

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