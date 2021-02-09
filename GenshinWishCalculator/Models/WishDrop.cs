using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GenshinWishCalculator
{
    public class Banner : INotifyPropertyChanged
    {
        private static DateTime genshinImpactStartTime = new DateTime(2020, 09, 28);
        private TimeSpan daysSince5Star = DateTime.Now - genshinImpactStartTime;
        private BannerType _bannerType;
        private BindingList<WishDrop> _wishList = new BindingList<WishDrop>();
        private int _pityLimit;
        private int _characterCount;// = wishList.Where(s => s != null && s.DropType.Equals(DropType.Character)).Count();
        // = wishList.Where(s => s != null && s.DropRarity.Equals(4)).Count();
        private int _fiveStarCount;// = wishList.Where(s => s != null && s.DropRarity.Equals(5)).Count();
        private int _wishesTill5Star;// wishList.FindIndex(s => s.DropRarity.Equals(5)) + 1;
                                     //try
                                     //{
                                     //    wishesTill5Star = wishList.Select((wish, index) => new { wish, index }).Where(s => s != null && s.wish.DropRarity.Equals(5)).First().index + 1;
                                     //}
                                     //catch (InvalidOperationException) { }

                                     //int wishCount = wishList.Count;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _UpdateField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, newValue))
            {
                return false;
            }

            T oldValue = field;

            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        

        //double characterPercentCount = (double)characterCount / (double)wishCount;
        //double fourStarPercentCount = (double)fourStarCount / (double)wishCount;
        //double fiveStarPercentCount = (double)fiveStarCount / (double)wishCount;

        //labelWishCount.Content = wishCount;
        //labelPrimogemsCount.Content = wishCount * 160;
        //labelCharacterCount.Content = characterCount;
        //labelCharacterPercentCount.Content = characterPercentCount.ToString("P2");
        //label4StarCount.Content = fourStarCount;
        //label4StarPercentCount.Content = fourStarPercentCount.ToString("P2");
        //label5StarCount.Content = fiveStarCount;
        //label5StarPercentCount.Content = fiveStarPercentCount.ToString("P2");
        //labelwishesTillPityCount.Content = activeBanner.PityLimit - wishesTill5Star;
        //labelwishesTillPityPrimogemsCount.Content = (activeBanner.PityLimit - wishesTill5Star) * 160;

        public BannerType BannerType
        {
            get => _bannerType;
            set
            {
                _bannerType = value;
                switch (BannerType)
                {
                    case BannerType.Character:
                        _pityLimit = 90;
                        break;
                    case BannerType.Weapon:
                        _pityLimit = 50;
                        break;
                    case BannerType.Standard:
                        _pityLimit = 90;
                        break;
                    case BannerType.Novice:
                        _pityLimit = 20;
                        break;
                    default:
                        break;
                }
            }
        }
        public BindingList<WishDrop> WishList { get => _wishList; set => _wishList = value; }
        public int PityLimit { get => _pityLimit; }
        public int CharacterCount { get => _wishList.Count; }
        public int FourStarCount { get => _wishList.Where(s => s != null && s.DropRarity.Equals(4)).Count(); }
        public int FiveStarCount { get => _wishList.Where(s => s != null && s.DropRarity.Equals(5)).Count(); }
        public int WishesTill5Star 
        { 
            get
            {
                int wishesTill5Star = _pityLimit;
                try
                {
                    wishesTill5Star = _wishList.Select((wish, index) => new { wish, index }).Where(s => s != null && s.wish.DropRarity.Equals(5)).First().index + 1;
                }
                catch (InvalidOperationException) { }
                return wishesTill5Star;
            }
        }

        public Banner() => _wishList = new BindingList<WishDrop>();
        public Banner(BannerType _bannerType) : this() => BannerType = _bannerType;
        public Banner(BannerType _bannerType, string _genshinInput) : this(_bannerType) => AddRange(_genshinInput);

        [JsonConstructor]
        public Banner(BannerType bannerType, BindingList<WishDrop> wishList) =>
            (BannerType, WishList) = (bannerType, wishList);

        public void AddRange(string input)
        {
            List<WishDrop> _wishList = new List<WishDrop>();
            using (StringReader reader = new StringReader(input))
            {
                string line = string.Empty;
                string wholeLine = string.Empty;
                do
                {
                    for (int i = 0; i < 3; i++)
                    {
                        line = reader.ReadLine();
                        wholeLine += line + "\n";
                    }

                    if (wholeLine != "\n\n\n")
                    {
                        _wishList.Add(new WishDrop(wholeLine));
                    }

                    wholeLine = string.Empty;
                } while (line != null);
                
            }

            for (int i = 0; i < _wishList.Count; i++)
            {
                this._wishList.Insert(i, _wishList[i]);
            }
        }
    }


    public class WishDrop
    {
        private string _dropName;
        private DropType _dropType;
        private DateTime _dropTime;
        private int _dropRarity;

        public string DropName { get => _dropName; set => _dropName = value; }
        public DropType DropType { get => _dropType; set => _dropType = value; }
        public DateTime DropTime { get => _dropTime; set => _dropTime = value; }
        public int DropRarity { get => _dropRarity; set => _dropRarity = value; }

        public WishDrop(DropType dropType, string dropName, DateTime dropTime)
        {
            DropType = dropType;
            DropName = dropName;
            DropTime = dropTime;
        }

        public WishDrop(string drop)
        {
            string line = string.Empty;
            using (StringReader reader = new StringReader(drop))
            {
                try
                {
                    //DropType
                    line = reader.ReadLine();
                    Enum.TryParse(line, out _dropType);

                    //DropName
                    line = reader.ReadLine();
                    DropName = line;

                    //DropRarity
                    if (int.TryParse(new string(_dropName.Where(char.IsDigit).ToArray()), out _dropRarity)) { }
                    else { _dropRarity = 3; };

                    //DropType
                    line = reader.ReadLine();
                    DateTime.TryParse(line, out _dropTime);
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }
        }

        [JsonConstructor]
        public WishDrop(string dropName, DropType dropType, DateTime dropTime, int dropRarity) =>
            (DropName, DropType, DropTime, DropRarity) = (dropName, dropType, dropTime, dropRarity);
    }

    public enum DropType
    {
        Character,
        Weapon,
    }

    public enum BannerType
    {
        Character,
        Weapon,
        Standard,
        Novice
    }
}
