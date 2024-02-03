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
    public class StatusBarViewModel : RegionViewModelBase
    {
        public StatusBarViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            startCountdownCommand = new DelegateCommand(StartCountdown);
            editRemainingTimeCommand = new DelegateCommand<string>(EditRemainingTime);
        }

        public ICommand EditRemainingTimeCommand => editRemainingTimeCommand;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
        public ICommand StartCountdownCommand => startCountdownCommand;
        public ResinTimer Timer { get => timer; set => SetProperty(ref timer, value); }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private readonly DelegateCommand<string> editRemainingTimeCommand;
        private readonly DelegateCommand startCountdownCommand;
        private bool isVisible;
        private ResinTimer timer = new ResinTimer();

        private void EditRemainingTime(string obj)
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

            Analytics.TrackEvent(nameof(ResinTimer), new Dictionary<string, string> { { "Action", "Edited" }, { "Amount", obj } });

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