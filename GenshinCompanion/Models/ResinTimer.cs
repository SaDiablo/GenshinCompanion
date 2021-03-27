using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenshinWishCalculator.Models
{
    public class ResinTimer : ModelBase
    {
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                EndTime = DateTime.UtcNow + value;
                Save();
                _UpdateField(ref _duration, new TimeSpan(0, 0, 0));
            }
        }

        private DateTime? _startTime;
        public DateTime? StartTime { get => _startTime; set => _UpdateField(ref _startTime, value); }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime { get => _remainingTime; set => _UpdateField(ref _remainingTime, value); }

        private DateTime? _endTime;
        public DateTime? EndTime { get => _endTime; 
            set 
            {
                if (value != _endTime) { Save(); }
                _UpdateField(ref _endTime, value); 
            } 
        }

        private bool _running;
        public bool Running { get => _running; set => _running = value; }

        public ResinTimer()
        {
            Open();
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
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GenshinCompanion", "Settings");
            string[] files = null;
            if (Directory.Exists(path))
                files = Directory.GetFiles(path);

            string filePath = Path.Combine(path, "Timer.json");
            if (files != null && files.Contains(filePath))
            {
                using (FileStream openStream = File.OpenRead(filePath))
                {
                    EndTime = await JsonSerializer.DeserializeAsync<DateTime>(openStream);
                }
                RaisePropertyChanged("EndTime");
                _StartCountdown();
            }
        }

        public void Save()
        {
            if (EndTime != null & EndTime.HasValue)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GenshinCompanion", "Settings");
                string filePath = Path.Combine(path, "Timer.json");

                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }

                using (StreamWriter file = File.CreateText(filePath))
                {
                    file.Write(JsonSerializer.Serialize(EndTime, options));
                }
            }
        }
    }
}
