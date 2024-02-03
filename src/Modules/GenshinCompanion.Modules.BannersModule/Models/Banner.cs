using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GenshinCompanion.CoreStandard.Enums;
using GenshinCompanion.CoreStandard.Interfaces;
using GenshinCompanion.Services;
using GenshinCompanion.Services.Enums;
using LiteDB;
using Prism.Mvvm;

namespace GenshinCompanion.Modules.BannersModule.Models
{
    public class Banner : BindableBase, IPersistData
    {
        #region Public Fields

        public static double[] fiveStarPercent = new double[]
        { 0.00600, 0.01196, 0.01788, 0.02379, 0.02965, 0.03547, 0.04126, 0.04701, 0.05272, 0.05840, 0.06405, 0.06966, 0.07524, 0.08078, 0.08630, 0.09179, 0.09724, 0.10265, 0.10804, 0.11340, 0.11871, 0.12399, 0.12924, 0.13447, 0.13966, 0.14483, 0.14996, 0.15507, 0.16014, 0.16517, 0.17018, 0.17516, 0.18011, 0.18502, 0.18991, 0.19477, 0.19960, 0.20440, 0.20917, 0.21392, 0.21863, 0.22332, 0.22799, 0.23263, 0.23724, 0.24181, 0.24637, 0.25090, 0.25538, 0.25985, 0.26430, 0.26872, 0.27311, 0.27748, 0.28182, 0.28612, 0.29040, 0.29466, 0.29889, 0.30309, 0.30727, 0.31143, 0.31556, 0.31966, 0.32374, 0.32780, 0.33184, 0.33585, 0.33984, 0.34380, 0.34773, 0.35165, 0.35553, 0.35940, 0.36324, 0.56951, 0.70897, 0.80146, 0.86521, 0.90827, 0.93741, 0.95711, 0.97043, 0.97944, 0.98552, 0.98963, 0.99241, 0.99428, 0.99554, 0.99819 };

        public static double[] fiveStarRate = new double[]
        {0.00600, 0.00596, 0.00592, 0.00591, 0.00586, 0.00582, 0.00579, 0.00575, 0.00571, 0.00568, 0.00565, 0.00561, 0.00558, 0.00554, 0.00552, 0.00549, 0.00545, 0.00541, 0.00539, 0.00536, 0.00531, 0.00528, 0.00525, 0.00523, 0.00519, 0.00517, 0.00513, 0.00511, 0.00507, 0.00503, 0.00501, 0.00498, 0.00495, 0.00491, 0.00489, 0.00486, 0.00483, 0.00480, 0.00477, 0.00475, 0.00471, 0.00469, 0.00467, 0.00464, 0.00461, 0.00457, 0.00456, 0.00453, 0.00448, 0.00447, 0.00445, 0.00442, 0.00439, 0.00437, 0.00434, 0.00430, 0.00428, 0.00426, 0.00423, 0.00420, 0.00418, 0.00416, 0.00413, 0.00410, 0.00408, 0.00406, 0.00404, 0.00401, 0.00399, 0.00396, 0.00393, 0.00392, 0.00388, 0.00387, 0.00384, 0.20627, 0.13946, 0.09249, 0.06375, 0.04306, 0.02914, 0.01970, 0.01332, 0.00901, 0.00608, 0.00411, 0.00278, 0.00187, 0.00126, 0.00265 };

        public static string[] fourStarPercent = new string[]
        { "0", "1", "2" };

        public static string[] fourStarRate = new string[]
        { "0", "1", "2" };
        #endregion Public Fields

        #region Private Fields

        private BannerType bannerType;
        private int pityLimit;
        private BindingList<WishDrop> wishList = new BindingList<WishDrop>();
        private List<int> fiveStarIndex = new List<int>();
        private readonly DateTime genshinImpactStartTime = new DateTime(2020, 09, 28);
        //private TimeSpan daysSince5Star = DateTime.Now - genshinImpactStartTime;

        #endregion Private Fields

        #region Public Constructors

        public Banner()
        {
            wishList = new BindingList<WishDrop>();
        }

        public Banner(BannerType bannerType) : this()
        {
            BannerType = bannerType;
        }

        public Banner(BannerType bannerType, string genshinInput) : this(bannerType)
        {
            AddRange(genshinInput);
        }

        [JsonConstructor]
        public Banner(BannerType bannerType, BindingList<WishDrop> wishList)
        {
            (BannerType, WishList) = (bannerType, wishList);
        }

        #endregion Public Constructors

        #region Public Properties

        public BannerType BannerType
        {
            get => bannerType;
            set
            {
                bannerType = value;
                switch (BannerType)
                {
                    case BannerType.Character:
                        pityLimit = 90;
                        break;

                    case BannerType.Weapon:
                        pityLimit = 80;
                        break;

                    case BannerType.Standard:
                        pityLimit = 90;
                        break;

                    case BannerType.Novice:
                        pityLimit = 20;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        public int CharacterCount => wishList.Count(s => s != null && s.DropType.Equals(DropType.Character));
        public string CharacterPercent
        {
            get
            {
                double _characterPercent = CharacterCount / (double)WishList.Count;
                return !double.IsNaN(_characterPercent) ? _characterPercent.ToString("P2") : "0";
            }
        }

        public int FiveStarCount => wishList.Count(s => s != null && s.DropRarity.Equals(5));
        public List<int> FiveStarIndex
        {
            get
            {
                if (fiveStarIndex.Count == 0)
                {
                    fiveStarIndex = RecountFiveStarIndexes();
                }

                return fiveStarIndex;
            }

            set => fiveStarIndex = value;
        }

        private List<int> RecountFiveStarIndexes()
        {
            var fiveStarIndex = new List<int>();
            var indexes = wishList
                .Select((wish, index) => new { wish, index })
                .Where(s => s != null && s.wish.DropRarity.Equals(5));
            foreach (var item in indexes)
            {
                fiveStarIndex.Add(item.index);
            }

            return fiveStarIndex;
        }

        public string FiveStarPercent
        {
            get
            {
                double fiveStarPercent = FiveStarCount / (double)WishList.Count;
                return !double.IsNaN(fiveStarPercent) ? fiveStarPercent.ToString("P2") : "0";
            }
        }

        public int FourStarCount => wishList.Count(s => s != null && s.DropRarity.Equals(4));
        public string FourStarPercent
        {
            get
            {
                double fourStarPercent = FourStarCount / (double)WishList.Count;
                return !double.IsNaN(fourStarPercent) ? fourStarPercent.ToString("P2") : "0";
            }
        }

        public string NextFiveStarPercent
        {
            get
            {
                double nextFiveStarPercent = fiveStarPercent[pityLimit - WishesTill5Star];
                return !double.IsNaN(nextFiveStarPercent) ? nextFiveStarPercent.ToString("P4") + " Chance" : "0";
            }
        }

        public string NextFiveStarRate
        {
            get
            {
                double nextFiveStarRate = fiveStarRate[pityLimit - WishesTill5Star];
                return !double.IsNaN(nextFiveStarRate) ? nextFiveStarRate.ToString("P4") : "0";
            }
        }

        public string NextFourStarPercent
        {
            get
            {
                double fiveStarPercent = FiveStarCount / (double)WishList.Count;
                return !double.IsNaN(fiveStarPercent) ? fiveStarPercent.ToString("P2") : "0";
            }
        }

        public string NextFourStarRate
        {
            get
            {
                double fiveStarPercent = FiveStarCount / (double)WishList.Count;
                return !double.IsNaN(fiveStarPercent) ? fiveStarPercent.ToString("P2") : "0";
            }
        }

        public int PityLimit => pityLimit;
        public string PrimogemCount => $"{TotalCount * 160} Primogems";
        public int TotalCount => wishList.Count;
        public int WishesTill5Star => pityLimit - Get5StarIndex(0);

        public string WishesTill5StarPrimogem => $"{WishesTill5Star * 160} Primogems";
        public BindingList<WishDrop> WishList { get => wishList; set => wishList = value; }

        #endregion Public Properties

        #region Public Methods

        public void AddRange(string input)
        {
            if (input is null)
            {
                return;
            }

            var _wishList = new List<WishDrop>();
            using (var reader = new StringReader(input))
            {
                string line = string.Empty;
                string wholeLine = string.Empty;
                do
                {
                    for (int i = 0; i < 4; i++)
                    {
                        line = reader.ReadLine();
                        wholeLine += line + "\n";
                    }

                    if (wholeLine != "\n\n\n\n")
                    {
                        _wishList.Add(new WishDrop(wholeLine));
                    }

                    wholeLine = string.Empty;
                } while (line != null);
            }

            for (int i = 0; i < _wishList.Count; i++)
            {
                wishList.Insert(i, _wishList[i]);
            }

            FiveStarIndex = RecountFiveStarIndexes();

            string filePath = DataProvider.GetFilePath($"{nameof(Banner)}", DataFolder.Banners, DataFormat.Db);
            using (var db = new LiteDatabase(filePath))
            {
                ILiteCollection<WishDrop> collection = db.GetCollection<WishDrop>($"{BannerType}{nameof(Banner)}");

                for (int i = 0; i < _wishList.Count; i++)
                {
                    _wishList[i].DropIndex = Get5StarIndex(i);
                }

                _wishList.Reverse();
                collection.InsertBulk(_wishList);
            }

            RefreshCounts();
        }

        public void RemoveItem(WishDrop wish)
        {
            if (WishList.Remove(wish))
            {
                RefreshCounts();
            }

            string filePath = DataProvider.GetFilePath($"{nameof(Banner)}", DataFolder.Banners, DataFormat.Db);
            using (var db = new LiteDatabase(filePath))
            {
                ILiteCollection<WishDrop> collection = db.GetCollection<WishDrop>($"{BannerType}{nameof(Banner)}");
                bool removed = collection.Delete(wish.Id);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private int Get5StarIndex(int indexToCheckFrom)
        {
            int index = FiveStarIndex.FirstOrDefault(s => s > indexToCheckFrom);
            return index == default ? wishList.Count - indexToCheckFrom : index - indexToCheckFrom;
        }

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

        private void SetIndexes()
        {
            for (int i = 0; i < TotalCount; i++)
            {
                wishList[i].DropIndex = Get5StarIndex(i);
            }
        }

        /// <summary>
        /// Pretty printed json in AppData/Local/GenshinCompanion/Banners folder
        /// </summary>
        public void Save()
        {
            //if (WishList != null && WishList.Any())
            //{
            //    DataProvider.Save(WishList, $"{BannerType}{nameof(Banner)}");
            //}
        }

        /// <summary>
        /// 1. Check if xBanner.json exist
        /// 2. Open Json
        /// 3. Initialize DB
        /// 4. Remove Json
        /// 5. Open DB
        /// </summary>
        /// <returns></returns>
        public async Task Open()
        {
            if (IsExistingJsonDatabase())
            {
                BindingList<WishDrop> deserializedData = await DataProvider.Open<BindingList<WishDrop>>($"{BannerType}{nameof(Banner)}");

                if (deserializedData != null || deserializedData != default)
                {
                    WishList = deserializedData;
                    SetIndexes();
                    RaisePropertyChanged(nameof(WishList));
                    RefreshCounts();
                    InitDB(WishList);
                    DataProvider.RemoveFile($"{BannerType}{nameof(Banner)}");
                }
            }

            List<WishDrop> list = OpenDB();
            WishList = new BindingList<WishDrop>(list);
            RaisePropertyChanged(nameof(WishList));
            RefreshCounts();
        }

        private List<WishDrop> OpenDB()
        {
            string filePath = DataProvider.GetFilePath($"{nameof(Banner)}", DataFolder.Banners, DataFormat.Db);

            using (var db = new LiteDatabase(filePath))
            {
                ILiteCollection<WishDrop> collection = db.GetCollection<WishDrop>($"{BannerType}{nameof(Banner)}");

                return collection.FindAll().Reverse().ToList();
            }
        }

        /// <summary>
        /// Used to convert to NoSQL database
        /// </summary>
        /// <returns></returns>
        private bool IsExistingJsonDatabase()
        {
            string folderPath = DataProvider.GetFolderPath(DataFolder.Banners);
            string filePath = DataProvider.GetFilePath($"{BannerType}{nameof(Banner)}", DataFolder.Banners, DataFormat.Json);

            string[] files = null;
            if (Directory.Exists(folderPath))
            {
                files = Directory.GetFiles(folderPath);
            }

            if (files != null && files.Contains(filePath, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public void InitDB(BindingList<WishDrop> wishList)
        {
            string filePath = DataProvider.GetFilePath($"{nameof(Banner)}", DataFolder.Banners, DataFormat.Db);

            using (var db = new LiteDatabase(filePath))
            {
                ILiteCollection<WishDrop> collection = db.GetCollection<WishDrop>($"{BannerType}{nameof(Banner)}");
                foreach (WishDrop drop in wishList.Reverse())
                {
                    if (drop.DropBannerName == null)
                    {
                        switch (BannerType)
                        {
                            case BannerType.Character:
                                drop.DropBannerName = "Character Event Wish";
                                break;
                            case BannerType.Weapon:
                                break;
                            case BannerType.Standard:
                                drop.DropBannerName = "Permanent Wish";
                                break;
                            case BannerType.Novice:
                                break;
                            default:
                                break;
                        }
                    }

                    collection.Insert(drop);
                }
            }
        }

        #endregion Private Methods
    }
}