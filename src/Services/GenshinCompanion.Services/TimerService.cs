using System;
using System.Timers;
using Prism.Mvvm;

namespace GenshinCompanion.Services
{
    public class TimerService : BindableBase
    {
        public TimerService()
        {
            timer = new System.Timers.Timer
            {
                Interval = 500
            };
            timer.Elapsed += OnTimedEvent;
        }

        public TimerService(DateTime? endTime, double interval = 500) : this()
        {
            EndTime = endTime;
            CalculateRemainingTime();
            timer.Interval = interval;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            CalculateRemainingTime();
            if (RemainingTime <= TimeSpan.Zero)
            {
                timer.Enabled = false;
                RemainingTime = TimeSpan.Zero;
            }
        }

        private void CalculateRemainingTime()
        {
            startTime = DateTime.UtcNow;
            RemainingTime = (EndTime - startTime) ?? TimeSpan.Zero;
        }

        private readonly System.Timers.Timer timer;
        private DateTime? startTime;
        private TimeSpan remainingTime;
        private DateTime? endTime;

        public DateTime? EndTime
        {
            get => endTime;
            set
            {
                if (SetProperty(ref endTime, value))
                {
                    if (endTime > DateTime.Now)
                    {
                        SetRunning(true);
                    }
                }
            }
        }
        public TimeSpan RemainingTime { get => remainingTime; set => SetProperty(ref remainingTime, value); }
        public double GetInterval()
        {
            return timer.Interval;
        }

        public void SetInterval(double value)
        {
            timer.Interval = value;
        }

        public bool GetRunning()
        {
            return timer.Enabled;
        }

        public void SetRunning(bool value)
        {
            timer.Enabled = value;
        }
    }
}
