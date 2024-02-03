using System;
using System.Collections.Generic;
using System.Windows.Input;
using GenshinCompanion.Core.Mvvm;
using GenshinCompanion.Modules.BannersModule.Models;
using GenshinCompanion.Services.Interfaces;
using Microsoft.AppCenter.Analytics;
using Prism.Commands;
using Prism.Regions;

namespace GenshinCompanion.Modules.BannersModule.ViewModels
{
    public class TimersViewModel : RegionViewModelBase
    {
        public TimersViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            startCountdownCommand = new DelegateCommand(StartCountdown);
            editRemainingTimeCommand = new DelegateCommand<string>(EditRemainingTime);
        }

        public ICommand EditRemainingTimeCommand => editRemainingTimeCommand;

        public ICommand StartCountdownCommand => startCountdownCommand;

        public ParametricTimer Timer { get => timer; set => SetProperty(ref timer, value); }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private readonly DelegateCommand<string> editRemainingTimeCommand;
        private readonly DelegateCommand startCountdownCommand;
        private ParametricTimer timer = new ParametricTimer();

        private void EditRemainingTime(string obj)
        {
            if (Timer.EndTime is null || Timer.EndTime.Value < DateTime.UtcNow)
            {
                Timer.EndTime = DateTime.UtcNow;
            }

            Analytics.TrackEvent(nameof(ParametricTimer), new Dictionary<string, string> { { "Action", "Edited" }, { "Amount", obj } });

            if (!Timer.Running)
            {
                Timer.StartCountdown();
            }

            Timer.Save();
        }

        private void StartCountdown()
        {
            Timer.StartCountdown();
        }
    }
}