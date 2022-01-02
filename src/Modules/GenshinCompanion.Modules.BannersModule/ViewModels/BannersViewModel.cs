using GenshinCompanion.Core.Mvvm;
using GenshinCompanion.CoreStandard.Enums;
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
    public class BannersViewModel : RegionViewModelBase
    {
        public BannersViewModel(IRegionManager regionManager, IMessageService messageService) : base(regionManager)
        {
            startCountdownCommand = new DelegateCommand(StartCountdown);
            editRemainingTimeCommand = new DelegateCommand<string>(EditRemainingTime);
            addWishesCommand = new DelegateCommand<string>(AddWishes);
            saveBannersCommand = new DelegateCommand(SaveBanners);
            openBannersCommand = new DelegateCommand(OpenBanners);
            minimizeWindowCommand = new DelegateCommand(MinimizeWindow);
            removeWishCommand = new DelegateCommand<WishDrop>(RemoveWish);
            OpenBanners();
        }

        public ICommand AddWishesCommand => addWishesCommand;
        public Banner CharacterBanner { get => characterBanner; set => SetProperty(ref characterBanner, value); }
        public ICommand EditRemainingTimeCommand => editRemainingTimeCommand;
        public string InputString { get => inputString; set => SetProperty(ref inputString, value); }
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
        public ICommand MinimizeWindowCommand => minimizeWindowCommand;
        public Banner NoviceBanner { get => noviceBanner; set => SetProperty(ref noviceBanner, value); }
        public ICommand OpenBannersCommand => openBannersCommand;
        public ICommand RemoveWishCommand => removeWishCommand;
        public ICommand SaveBannersCommand => saveBannersCommand;
        public Banner StandardBanner { get => standardBanner; set => SetProperty(ref standardBanner, value); }
        public ICommand StartCountdownCommand => startCountdownCommand;
        public int TabBannersIndex { get => tabBannersIndex; set => SetProperty(ref tabBannersIndex, value); }
        public ResinTimer Timer { get => timer; set => SetProperty(ref timer, value); }
        public Banner WeaponBanner { get => weaponBanner; set => SetProperty(ref weaponBanner, value); }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private readonly DelegateCommand<string> addWishesCommand;
        private readonly DelegateCommand<string> editRemainingTimeCommand;
        private readonly DelegateCommand minimizeWindowCommand;
        private readonly DelegateCommand openBannersCommand;
        private readonly DelegateCommand<WishDrop> removeWishCommand;
        private readonly DelegateCommand saveBannersCommand;
        private readonly DelegateCommand startCountdownCommand;
        private Banner characterBanner = new Banner(BannerType.Character);
        private string inputString;
        private bool isVisible;
        private Banner noviceBanner = new Banner(BannerType.Novice);
        private Banner standardBanner = new Banner(BannerType.Standard);
        private int tabBannersIndex;
        private ResinTimer timer = new ResinTimer();
        private Banner weaponBanner = new Banner(BannerType.Weapon);

        private void AddWishes(string bannerType)
        {
            switch (bannerType)
            {
                case "Character":
                    CharacterBanner.AddRange(InputString);
                    break;

                case "Weapon":
                    WeaponBanner.AddRange(InputString);
                    break;

                case "Standard":
                    StandardBanner.AddRange(InputString);
                    break;

                case "Novice":
                    NoviceBanner.AddRange(InputString);
                    break;

                default:
                    break;
            }
            Analytics.TrackEvent("Wish", new Dictionary<string, string> { { "Action", "Added" }, { "Category", bannerType } });
            InputString = string.Empty;
            SaveBanners();
        }

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

            Analytics.TrackEvent("Timer", new Dictionary<string, string> { { "Action", "Edited" }, { "Amount", obj } });

            if (!Timer.Running)
            {
                Timer.StartCountdown();
            }

            Timer.Save();
        }

        private void MinimizeWindow()
        {
        }

        private async void OpenBanners()
        {
            await CharacterBanner.Open();
            await WeaponBanner.Open();
            await StandardBanner.Open();
            await NoviceBanner.Open();
        }

        private void RemoveWish(WishDrop wish)
        {
            string bannerType = "Default";

            if (CharacterBanner.WishList.Contains(wish)) { CharacterBanner.RemoveItem(wish); bannerType = "Character"; }
            if (WeaponBanner.WishList.Contains(wish)) { WeaponBanner.RemoveItem(wish); bannerType = "Weapon"; }
            if (StandardBanner.WishList.Contains(wish)) { StandardBanner.RemoveItem(wish); bannerType = "Standard"; }
            if (NoviceBanner.WishList.Contains(wish)) { NoviceBanner.RemoveItem(wish); bannerType = "Novice"; }

            Analytics.TrackEvent("Wish", new Dictionary<string, string> { { "Action", "Removed" }, { "Category", bannerType } });
            SaveBanners();
        }

        private void SaveBanners()
        {
            CharacterBanner.Save();
            WeaponBanner.Save();
            StandardBanner.Save();
            NoviceBanner.Save();
        }

        private void StartCountdown() => Timer.StartCountdown();
    }
}