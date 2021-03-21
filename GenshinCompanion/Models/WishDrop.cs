using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace GenshinWishCalculator.Models
{
    public class WishDrop : ModelBase
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
