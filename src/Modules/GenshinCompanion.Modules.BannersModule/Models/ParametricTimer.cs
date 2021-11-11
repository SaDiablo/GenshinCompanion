using GenshinCompanion.CoreStandard;
using GenshinCompanion.Services;
using Microsoft.AppCenter.Analytics;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenshinCompanion.Modules.BannersModule.Models
{
    public class ParametricTimer : BindableBase, IPersistData
    {
        public ParametricTimer() => Open();

        public TimeSpan Duration
        {
            get => new TimeSpan(7, 0, 0, 0);
            set
            {
                TimerService.EndTime = DateTime.UtcNow + value;
                Save();
                if (!Running) { StartCountdown(); }
            }
        }

        public DateTime? EndTime
        {
            get => endTime;
            set
            {
                if (SetProperty(ref endTime, value))
                {
                    if (TimerService != null) { TimerService.EndTime = value; }
                    Save();
                }
            }
        }

        public TimeSpan RemainingTime => TimerService != null ? TimerService.RemainingTime : TimeSpan.Zero;
        public bool Running => TimerService.GetRunning();
        public TimerService TimerService { get => timerService; set => SetProperty(ref timerService, value); }
        private DateTime? endTime;
        private TimerService timerService;

        public async void Open()
        {
            object deserializedData = await DataProvider.Open<DateTime?>(nameof(ParametricTimer), DataFolder.Settings);

            if (deserializedData != null)
            {
                EndTime = (DateTime?)deserializedData;
                TimerService = new TimerService(EndTime, 30000);
            }
            else
            {
                TimerService = new TimerService(DateTime.UtcNow, 30000);
            }
        }

        public void Save()
        {
            if (TimerService != null && TimerService.EndTime != null && TimerService.EndTime.HasValue)
            {
                DataProvider.Save(TimerService.EndTime, nameof(ParametricTimer), DataFolder.Settings);
            }
        }

        public void StartCountdown()
        {
            Analytics.TrackEvent("Timer", new Dictionary<string, string> { { "Action", "Started" } });
            TimerService.SetRunning(true);
        }
    }
}