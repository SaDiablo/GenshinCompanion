using Prism.Mvvm;
using System;
using System.Timers;

namespace GenshinCompanion.Services
{
    public class TimerService : BindableBase
    {
        public TimerService(DateTime? endTime, double interval = 500)
        {
            EndTime = endTime;
            timer = new Timer
            {
                Interval = interval,
                AutoReset = true,
            };

            // Hook up the Elapsed event for the timer.
            timer.Elapsed += OnTimedEvent;

            // Start the timer
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //startTime = DateTime.UtcNow;
            startTime = e.SignalTime.ToUniversalTime();
            RemainingTime = (EndTime - startTime) ?? TimeSpan.Zero;
            if (RemainingTime <= TimeSpan.Zero)
            {
                timer.Enabled = false;
                RemainingTime = TimeSpan.Zero;
            }
        }

        private readonly Timer timer;
        private DateTime? startTime;
        private TimeSpan remainingTime;
        private DateTime? endTime;

        public DateTime? EndTime { get => endTime; set => SetProperty(ref endTime, value); }
        public TimeSpan RemainingTime { get => remainingTime; set => SetProperty(ref remainingTime, value); }
        public double GetInterval() => timer.Interval;
        public void SetInterval(double value) => timer.Interval = value;
        public bool GetRunning() => timer.Enabled;
        public void SetRunning(bool value) => timer.Enabled = value;
    }
}
