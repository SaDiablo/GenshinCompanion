using GenshinCompanion.CoreStandard.Enums;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace GenshinCompanion.Modules.BannersModule.Models
{
    public class WishDrop
    {
        private string _dropName;
        private DropType _dropType;
        private DateTime _dropTime;
        private int _dropRarity;
        private int _dropIndex;
        private string dropBannerName;

        public int Id { get; set; }
        public string DropName { get => _dropName; set => _dropName = value; }
        public string DropBannerName { get => dropBannerName; set => dropBannerName = value; }
        public DropType DropType { get => _dropType; set => _dropType = value; }
        public DateTime DropTime { get => _dropTime; set => _dropTime = value; }
        public int DropRarity { get => _dropRarity; set => _dropRarity = value; }
        public int DropIndex { get => _dropIndex; set => _dropIndex = value; }

        public WishDrop(DropType dropType, string dropName, DateTime dropTime)
        {
            DropType = dropType;
            DropName = dropName;
            DropTime = dropTime;
        }

        public WishDrop(DropType dropType, string dropName, string dropBannerName, DateTime dropTime)
        {
            DropType = dropType;
            DropName = dropName;
            DropBannerName = dropBannerName;
            DropTime = dropTime;
        }

        public WishDrop(string drop)
        {
            // Add better Exception handling
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

                    //DropBannerName
                    line = reader.ReadLine();
                    DropBannerName = line;

                    //DropType
                    line = reader.ReadLine();
                    DateTime.TryParse(line, out _dropTime);
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        
        [JsonConstructor]
        public WishDrop(string dropName, DropType dropType, DateTime dropTime, int dropRarity, string dropBannerName) =>
            (DropName, DropType, DropTime, DropRarity, DropBannerName) = (dropName, dropType, dropTime, dropRarity, dropBannerName);
    }
}