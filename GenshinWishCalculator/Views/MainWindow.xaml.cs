﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using System.Collections.ObjectModel;
using GenshinWishCalculator.ViewModels;

namespace GenshinWishCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();
        //public static Banner characterBanner = new Banner(Type.Character);
        //public static Banner weaponBanner = new Banner(Type.Weapon);
        //public static Banner standardBanner = new Banner(Type.Standard);
        //public static Banner noviceBanner = new Banner(Type.Novice);

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainWindowViewModel;
        }

        private void bannerDisplay(ListView listView, Banner activeBanner)
        {
            if(activeBanner != null)
            {
                //listView.ItemsSource = activeBanner.WishList;
                //listView.UpdateLayout();
                updateWishNumbers(activeBanner);
            }
        }

        private void updateWishNumbers(Banner activeBanner)
        {
            if (activeBanner != null & activeBanner.WishList.Count > 0)
            {
                //todo
                int wishCount = activeBanner.WishList.Count;
                int characterCount = activeBanner.WishList.Where(s => s != null && s.DropType.Equals(DropType.Character)).Count();
                int fourStarCount = activeBanner.WishList.Where(s => s != null && s.DropRarity.Equals(4)).Count();
                int fiveStarCount = activeBanner.WishList.Where(s => s != null && s.DropRarity.Equals(5)).Count();
                //check if exists
                //int wishesTill5Star = activeBanner.WishList.FindIndex(s => s.DropRarity.Equals(5)) + 1;
                int wishesTill5Star = activeBanner.WishList.Count;
                try
                {
                    wishesTill5Star = activeBanner.WishList.Select((wish, index) => new { wish, index }).Where(s => s != null && s.wish.DropRarity.Equals(5)).First().index + 1;
                }
                catch (InvalidOperationException) { }
                
                double characterPercentCount = (double)characterCount / (double)wishCount;
                double fourStarPercentCount = (double)fourStarCount / (double)wishCount;
                double fiveStarPercentCount = (double)fiveStarCount / (double)wishCount;

                labelWishCount.Content = wishCount;
                labelPrimogemsCount.Content = wishCount * 160;
                labelCharacterCount.Content = characterCount;
                labelCharacterPercentCount.Content = characterPercentCount.ToString("P2");
                label4StarCount.Content = fourStarCount;
                label4StarPercentCount.Content = fourStarPercentCount.ToString("P2");
                label5StarCount.Content = fiveStarCount;
                label5StarPercentCount.Content = fiveStarPercentCount.ToString("P2");
                labelwishesTillPityCount.Content = activeBanner.PityLimit - wishesTill5Star;
                labelwishesTillPityPrimogemsCount.Content = (activeBanner.PityLimit - wishesTill5Star) * 160;
            }
        }

        private async void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream openStream = File.OpenRead("characterBanner.json"))
                {
                    _mainWindowViewModel.CharacterBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
            try
            {
                using (FileStream openStream = File.OpenRead("weaponBanner.json"))
                {
                    _mainWindowViewModel.WeaponBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
            try
            {
                using (FileStream openStream = File.OpenRead("standardBanner.json"))
                {
                    _mainWindowViewModel.StandardBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }
            try
            {
                using (FileStream openStream = File.OpenRead("noviceBanner.json"))
                {
                    _mainWindowViewModel.NoviceBanner = await JsonSerializer.DeserializeAsync<Banner>(openStream);
                }
            }
            catch (FileNotFoundException) { }

            //switch (tabControlBanners.SelectedIndex)
            //{
            //    case 0:
            //        bannerDisplay(listViewCharacterBanner, characterBanner);
            //        break;
            //    case 1:
            //        bannerDisplay(listViewWeaponBanner, weaponBanner);
            //        break;
            //    case 2:
            //        bannerDisplay(listViewStandardBanner, standardBanner);
            //        break;
            //    case 3:
            //        bannerDisplay(listViewNoviceBanner, noviceBanner);
            //        break;
            //    default:
            //        break;
            //}
        }

        private void menuItemSave_Click(object sender, RoutedEventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            if (_mainWindowViewModel.CharacterBanner != null & _mainWindowViewModel.CharacterBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("characterBanner.json"))
                {
                    var json = JsonSerializer.Serialize(_mainWindowViewModel.CharacterBanner, options);
                    file.Write(json);
                }
            }
            if (_mainWindowViewModel.WeaponBanner != null & _mainWindowViewModel.WeaponBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("weaponBanner.json"))
                {
                    var json = JsonSerializer.Serialize(_mainWindowViewModel.WeaponBanner, options);
                    file.Write(json);
                }
            }
            if (_mainWindowViewModel.StandardBanner != null & _mainWindowViewModel.StandardBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("standardBanner.json"))
                {
                    var json = JsonSerializer.Serialize(_mainWindowViewModel.StandardBanner, options);
                    file.Write(json);
                }
            }
            if (_mainWindowViewModel.NoviceBanner != null & _mainWindowViewModel.NoviceBanner.WishList.Count > 0)
            {
                using (StreamWriter file = File.CreateText("noviceBanner.json"))
                {
                    var json = JsonSerializer.Serialize(_mainWindowViewModel.NoviceBanner, options);
                    file.Write(json);
                }
            }
        }

        private void buttonAddInput_Click(object sender, RoutedEventArgs e)
        {
            switch (tabControlBanners.SelectedIndex)
            {
                case 0:
                    _mainWindowViewModel.CharacterBanner.AddRange(textBoxBannerInput.Text);
                    updateWishNumbers(_mainWindowViewModel.CharacterBanner);
                    break;
                case 1:
                    _mainWindowViewModel.WeaponBanner.AddRange(textBoxBannerInput.Text);
                    updateWishNumbers(_mainWindowViewModel.WeaponBanner);
                    break;
                case 2:
                    _mainWindowViewModel.StandardBanner.AddRange(textBoxBannerInput.Text);
                    updateWishNumbers(_mainWindowViewModel.StandardBanner);
                    break;
                case 3:
                    _mainWindowViewModel.NoviceBanner.AddRange(textBoxBannerInput.Text);
                    updateWishNumbers(_mainWindowViewModel.NoviceBanner);
                    break;
                default:
                    break;
            }
            textBoxBannerInput.Text = "";
        }

        private void tabControlBanners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabControlBanners.SelectedIndex)
            {
                case 0:
                    updateWishNumbers(_mainWindowViewModel.CharacterBanner);
                    break;
                case 1:
                    updateWishNumbers(_mainWindowViewModel.WeaponBanner);
                    break;
                case 2:
                    updateWishNumbers(_mainWindowViewModel.StandardBanner);
                    break;
                case 3:
                    updateWishNumbers(_mainWindowViewModel.NoviceBanner);
                    break;
                default:
                    break;
            }
        }
    }
}
