using GenshinCompanion.CoreStandard;
using GenshinCompanion.Services;
using Microsoft.AppCenter.Analytics;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenshinCompanion.Modules.BannersModule.Models
{
    public class ResinTimer : BindableBase, IPersistData
    {
        public ResinTimer() => Open();

        public TimeSpan Duration
        {
            get => duration;
            set
            {
                TimerService.EndTime = DateTime.UtcNow + value;
                Save();
                SetProperty(ref duration, new TimeSpan(0, 0, 0));
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

        public DateTime? EndTime120 => TimerService.EndTime.Value.AddMinutes(-320);
        public DateTime? EndTime20 => TimerService.EndTime.Value.AddMinutes(-1120);
        public DateTime? EndTime40 => TimerService.EndTime.Value.AddMinutes(-960);
        public DateTime? EndTime80 => TimerService.EndTime.Value.AddMinutes(-640);
        public List<EndTime> EndTimes { get => endTimes; set => SetProperty(ref endTimes, value); }
        public TimeSpan RemainingTime => TimerService != null ? TimerService.RemainingTime : TimeSpan.Zero;
        public bool Running => TimerService.GetRunning();
        public DateTime? StartTime { get => startTime; set => SetProperty(ref startTime, value); }
        public TimerService TimerService { get => timerService; set => SetProperty(ref timerService, value); }
        private TimeSpan duration;
        private DateTime? endTime;
        private List<EndTime> endTimes;
        private DateTime? startTime;
        private TimerService timerService;

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

        public void StartCountdown()
        {
            Analytics.TrackEvent("Timer", new Dictionary<string, string> { { "Action", "Started" } });
            TimerService.SetRunning(true);
        }

        private EndTime GetEndTime(int resin) => EndTimes.Find((s) => s.Resin == resin);

        private void SetEndTime(DateTime? time, int Resin)
        {
            if (EndTimes.Exists((s) => s.Resin == Resin))
            {
                var endTime = EndTimes.FirstOrDefault((s) => s.Resin == Resin);
                if (endTime != null) endTime.Time = time;
            }
            else
            {
                EndTimes.Add(new EndTime(time, Resin));
            }
        }
    }
}