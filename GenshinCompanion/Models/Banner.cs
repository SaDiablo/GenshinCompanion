using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenshinWishCalculator.Models
{
    public class Banner : ModelBase
    {
        private BindingList<WishDrop> _wishList = new BindingList<WishDrop>();
        private BannerType _bannerType;
        private int _pityLimit;

        public static string[] _fourStarRate = new string[] 
        { "0", "1", "2" };
        public static double[] _fiveStarRate = new double[] 
        {0.00600, 0.00596, 0.00592, 0.00591, 0.00586, 0.00582, 0.00579, 0.00575, 0.00571, 0.00568, 0.00565, 0.00561, 0.00558, 0.00554, 0.00552, 0.00549, 0.00545, 0.00541, 0.00539, 0.00536, 0.00531, 0.00528, 0.00525, 0.00523, 0.00519, 0.00517, 0.00513, 0.00511, 0.00507, 0.00503, 0.00501, 0.00498, 0.00495, 0.00491, 0.00489, 0.00486, 0.00483, 0.00480, 0.00477, 0.00475, 0.00471, 0.00469, 0.00467, 0.00464, 0.00461, 0.00457, 0.00456, 0.00453, 0.00448, 0.00447, 0.00445, 0.00442, 0.00439, 0.00437, 0.00434, 0.00430, 0.00428, 0.00426, 0.00423, 0.00420, 0.00418, 0.00416, 0.00413, 0.00410, 0.00408, 0.00406, 0.00404, 0.00401, 0.00399, 0.00396, 0.00393, 0.00392, 0.00388, 0.00387, 0.00384, 0.20627, 0.13946, 0.09249, 0.06375, 0.04306, 0.02914, 0.01970, 0.01332, 0.00901, 0.00608, 0.00411, 0.00278, 0.00187, 0.00126, 0.00265 };
        public static string[] _fourStarPercent = new string[] 
        { "0", "1", "2" };
        public static double[] _fiveStarPercent = new double[] 
        { 0.00600, 0.01196, 0.01788, 0.02379, 0.02965, 0.03547, 0.04126, 0.04701, 0.05272, 0.05840, 0.06405, 0.06966, 0.07524, 0.08078, 0.08630, 0.09179, 0.09724, 0.10265, 0.10804, 0.11340, 0.11871, 0.12399, 0.12924, 0.13447, 0.13966, 0.14483, 0.14996, 0.15507, 0.16014, 0.16517, 0.17018, 0.17516, 0.18011, 0.18502, 0.18991, 0.19477, 0.19960, 0.20440, 0.20917, 0.21392, 0.21863, 0.22332, 0.22799, 0.23263, 0.23724, 0.24181, 0.24637, 0.25090, 0.25538, 0.25985, 0.26430, 0.26872, 0.27311, 0.27748, 0.28182, 0.28612, 0.29040, 0.29466, 0.29889, 0.30309, 0.30727, 0.31143, 0.31556, 0.31966, 0.32374, 0.32780, 0.33184, 0.33585, 0.33984, 0.34380, 0.34773, 0.35165, 0.35553, 0.35940, 0.36324, 0.56951, 0.70897, 0.80146, 0.86521, 0.90827, 0.93741, 0.95711, 0.97043, 0.97944, 0.98552, 0.98963, 0.99241, 0.99428, 0.99554, 0.99819 };
        private DateTime genshinImpactStartTime = new DateTime(2020, 09, 28);
        //private TimeSpan daysSince5Star = DateTime.Now - genshinImpactStartTime;

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
                        _pityLimit = 80;
                        break;
                    case BannerType.Standard:
                        _pityLimit = 90;
                        break;
                    case BannerType.Novice:
                        _pityLimit = 20;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }
        public BindingList<WishDrop> WishList { get => _wishList; set => _wishList = value; }
        public int PityLimit => _pityLimit;

        public int TotalCount => _wishList.Count;
        public string PrimogemCount => (TotalCount * 160).ToString() + " Primogems";
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
                    wishesTill5Star -= _wishList.Select((wish, index) => new { wish, index }).Where(s => s != null && s.wish.DropRarity.Equals(5)).First().index;
                }
                catch (InvalidOperationException) { wishesTill5Star = _pityLimit - _wishList.Count; }
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
        public string WishesTill5StarPrimogem => (WishesTill5Star * 160).ToString() + " Primogems";

        public string NextFourStarRate
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
        public string NextFiveStarRate
        {
            get
            {
                double _nextFiveStarRate = _fiveStarRate[_pityLimit - WishesTill5Star];
                if (Double.IsNaN(_nextFiveStarRate))
                    return "0";
                else
                    return _nextFiveStarRate.ToString("P4");
            }
        }
        public string NextFourStarPercent
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
        public string NextFiveStarPercent
        {
            get
            {
                double _nextFiveStarPercent = _fiveStarPercent[_pityLimit - WishesTill5Star];
                if (Double.IsNaN(_nextFiveStarPercent))
                    return "0";
                else
                    return _nextFiveStarPercent.ToString("P4") + " Chance";
            }
        }

        public Banner() => _wishList = new BindingList<WishDrop>();
        public Banner(BannerType _bannerType) : this() { BannerType = _bannerType; }
        public Banner(BannerType _bannerType, string _genshinInput) : this(_bannerType) => AddRange(_genshinInput);

        [JsonConstructor]
        public Banner(BannerType bannerType, BindingList<WishDrop> wishList) =>
            (BannerType, WishList) = (bannerType, wishList);


        private void RefreshCounts()
        {
            RaisePropertyChanged(nameof(TotalCount));
            RaisePropertyChanged(nameof(TotalCount));
            RaisePropertyChanged(nameof(PrimogemCount));

            RaisePropertyChanged(nameof(CharacterCount));
            RaisePropertyChanged(nameof(FourStarCount));
            RaisePropertyChanged(nameof(FiveStarCount));
            RaisePropertyChanged(nameof(WishesTill5Star));

            RaisePropertyChanged(nameof(CharacterPercent));
            RaisePropertyChanged(nameof(FourStarPercent));
            RaisePropertyChanged(nameof(FiveStarPercent));
            RaisePropertyChanged(nameof(WishesTill5StarPrimogem));

            RaisePropertyChanged(nameof(NextFourStarRate));
            RaisePropertyChanged(nameof(NextFiveStarRate));
            RaisePropertyChanged(nameof(NextFourStarPercent));
            RaisePropertyChanged(nameof(NextFiveStarPercent));
        }

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

        public void RemoveItem(WishDrop wish)
        {
            WishList.Remove(wish);
            RefreshCounts();
        }

        public async void Open()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    + "\\GenshinCompanion\\Banners\\";
            string[] files = null;
            if (Directory.Exists(path))
                files = Directory.GetFiles(path);

            //to check if bannerType has been initialized 
            string filePath = path + _bannerType.ToString().ToLower() + "Banner.json";
            if (files != null && files.Contains(filePath))
            {
                using (FileStream openStream = File.OpenRead(filePath))
                {
                    WishList = await JsonSerializer.DeserializeAsync<BindingList<WishDrop>>(openStream);
                }
                RaisePropertyChanged(nameof(WishList));
                RefreshCounts();
            }
        }

        /// <summary>
        /// Pretty printed json in AppData/Local/GenshinCompaion/Banners folder
        /// </summary>
        /// <returns>If saving was sucessful</returns>
        public bool TrySaveJson()
        {
            //TODO:
            //Don't hardcode path, check earlier for availability to save in folder
            //To make it nondestructive to older saves
            //Keep backups and check differences and then save
            //Do it asynchronically
            //make it format agnostic
            //make it platform agnostic
            //make the format a enum to choose from 
            if (WishList != null & WishList.Any())
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    + "\\GenshinCompanion\\Banners\\";
                string filePath = path + _bannerType.ToString().ToLower() + "Banner.json";

                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }

                using (StreamWriter file = File.CreateText(filePath))
                {
                    file.Write(JsonSerializer.Serialize(_wishList, options));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save banner in the same format as would be from copying wish history from game
        /// </summary>
        /// <param name="_outputPath">Path where to save .txt</param>
        /// <returns>If saving was sucessful</returns>
        public bool TrySaveTxt(string _outputPath)
        {
            //Do it asynchronically
            if (WishList != null & WishList.Count > 0)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    + "\\GenshinCompanion\\Banners\\";
                string filePath = path + _bannerType.ToString().ToLower() + "Banner.json";

                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }

                using (StreamWriter file = File.CreateText(filePath))
                {
                    file.Write(JsonSerializer.Serialize(_wishList, options));
                }
                return true;
            }
            return false;
        }
    }

    
}
