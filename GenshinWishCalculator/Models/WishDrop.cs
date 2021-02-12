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
        private BindingList<WishDrop> _wishList = new BindingList<WishDrop>();
        private BannerType _bannerType;
        private int _pityLimit;

        private static DateTime genshinImpactStartTime = new DateTime(2020, 09, 28);
        private TimeSpan daysSince5Star = DateTime.Now - genshinImpactStartTime;

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
        public int PityLimit => _pityLimit;

        public int TotalCount => _wishList.Count;
        public int PrimogemCount => TotalCount * 160;
        public int CharacterCount => _wishList.Where(s => s != null && s.DropType.Equals(DropType.Character)).Count();
        public int FourStarCount => _wishList.Where(s => s != null && s.DropRarity.Equals(4)).Count();
        public int FiveStarCount => _wishList.Where(s => s != null && s.DropRarity.Equals(5)).Count();
        public int WishesTill5Star
        {
            get
            {
                int wishesTill5Star = _pityLimit;
                try
                {
                    wishesTill5Star -= _wishList.Select((wish, index) => new { wish, index }).Where(s => s != null && s.wish.DropRarity.Equals(5)).First().index + 1;
                }
                catch (InvalidOperationException) { }
                return wishesTill5Star;
            }
        }

        public string CharacterPercent
        {
            get
            {
                double _characterPercent = (double)CharacterCount / (double)WishList.Count;
                if (Double.IsNaN(_characterPercent))
                    return "0";
                else
                    return _characterPercent.ToString("P2");
            }
        }
        public string FourStarPercent
        {
            get
            {
                double _fourStarPercent = (double)FourStarCount / (double)WishList.Count;
                if (Double.IsNaN(_fourStarPercent))
                    return "0";
                else
                    return _fourStarPercent.ToString("P2");
            }
        }
        public string FiveStarPercent
        {
            get
            {
                double _fiveStarPercent = (double)FiveStarCount / (double)WishList.Count;
                if (Double.IsNaN(_fiveStarPercent))
                    return "0";
                else
                    return _fiveStarPercent.ToString("P2");
            }
        }
        public int WishesTill5StarPrimogem => WishesTill5Star * 160;

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
            RefreshCounts();
        }

        private void RefreshCounts()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrimogemCount"));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CharacterCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FourStarCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FiveStarCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WishesTill5Star"));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CharacterPercentCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FourStarPercentCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FiveStarPercentCount"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WishesTill5StarPercent"));
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
