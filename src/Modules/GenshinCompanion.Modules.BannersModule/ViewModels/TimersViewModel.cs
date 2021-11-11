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
    public class TimersViewModel : RegionViewModelBase
    {
        public TimersViewModel(IRegionManager regionManager, IMessageService messageService) :
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

        private ParametricTimer _timer = new ParametricTimer();
        public ParametricTimer Timer { get => _timer; set => SetProperty(ref _timer, value); }

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

            Analytics.TrackEvent("ParametricTimer", new Dictionary<string, string> { { "Action", "Edited" }, { "Amount", obj } });

            if (!Timer.Running)
            {
                Timer.StartCountdown();
            }

            Timer.Save();
        }

        private void StartCountdown() => Timer.StartCountdown();
    }
}