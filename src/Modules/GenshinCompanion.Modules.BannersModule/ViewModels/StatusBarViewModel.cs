using GenshinCompanion.Core.Mvvm;
using GenshinCompanion.Modules.BannersModule.Models;
using GenshinCompanion.Services.Interfaces;
using Microsoft.AppCenter.Analytics;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GenshinCompanion.Modules.BannersModule.ViewModels
{
    public class StatusBarViewModel : RegionViewModelBase
    {
        public StatusBarViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            try
            {
                _startCountdownCommand = new DelegateCommand(StartCountdown);
                _editRemainingTimeCommand = new DelegateCommand<string>(_EditRemainingTime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private bool _isVisible;
        public bool IsVisible { get => _isVisible; set => SetProperty(ref _isVisible, value); }

        private ResinTimer _timer = new ResinTimer();
        public ResinTimer Timer { get => _timer; set => SetProperty(ref _timer, value); }

        private readonly DelegateCommand _startCountdownCommand;
        public ICommand StartCountdownCommand => _startCountdownCommand;

        private readonly DelegateCommand<string> _editRemainingTimeCommand;
        public ICommand EditRemainingTimeCommand => _editRemainingTimeCommand;

        private void _EditRemainingTime(string obj)
        {
            if (Timer.EndTime is null || Timer.EndTime.Value < DateTime.UtcNow)
            {
                Timer.EndTime = DateTime.UtcNow;
            }

            // Deal with negative numbers/stopping the timer/reseting it
            switch (obj)
            {
                case "-20":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(160);
                    break;
                case "+20":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(-160);
                    break;
                case "-10":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(80);
                    break;
                case "+10":
                    Timer.EndTime = Timer.EndTime.Value.AddMinutes(-80);
                    break;
            }

            Analytics.TrackEvent("Timer", new Dictionary<string, string> { { "Action", "Edited" }, { "Amount", obj } });

            if (!Timer.Running)
            {
                Timer.StartCountdown();
            }

            Timer.Save();
        }

        private void StartCountdown() => Timer.StartCountdown();
    }
}