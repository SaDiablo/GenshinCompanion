using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GenshinWishCalculator
{
    public class Banner
    {
        private Type bannerType;
        private static DateTime genshinImpactStartTime = new DateTime(2020, 09, 28);
        public List<WishDrop> wishList;
        private int pityLimit;
        public TimeSpan daysSince5Star = DateTime.Now - genshinImpactStartTime;

        public Type BannerType
        {
            get => bannerType;
            set
            {
                bannerType = value;
                switch (BannerType)
                {
                    case Type.Character:
                        pityLimit = 90;
                        break;
                    case Type.Weapon:
                        pityLimit = 50;
                        break;
                    case Type.Standard:
                        pityLimit = 90;
                        break;
                    case Type.Novice:
                        pityLimit = 20;
                        break;
                    default:
                        break;
                }
            }
        }

        public Banner(string _genshinInput, Type _bannerType) : this(_genshinInput)
        {
            BannerType = _bannerType;
        }

        public Banner(string _genshinInput)
        {
            wishList = new List<WishDrop>();
            using (StringReader reader = new StringReader(_genshinInput))
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
                        wishList.Add(new WishDrop(wholeLine));
                    }

                    wholeLine = string.Empty;
                } while (line != null);
            }
        }

    }


    public class WishDrop
    {
        private string dropName;
        private DropType dropType;
        private DateTime dropTime;
        public int dropRarity;

        public string DropName
        {
            get => dropName;
            set
            {
                dropName = value;
                /*
                 * System.Text.RegularExpressions.Regex
                 * resultString = Regex.Match(subjectString, @"\d+").Value;
                 * returns a string containing the first occurrence of a number in subjectString.
                 * Int32.Parse(resultString) will then give you the number.
                 */
                if (int.TryParse(new String(dropName.Where(char.IsDigit).ToArray()), out dropRarity)) { }
                else { dropRarity = 3; };
            }
        }

        public DropType DropType { get => dropType; set => dropType = value; }
        public DateTime DropTime { get => dropTime; set => dropTime = value; }

        public WishDrop(DropType _dropType, string _dropName, DateTime _dropTime)
        {
            DropType = _dropType;
            DropName = _dropName;
            DropTime = _dropTime;
        }

        public WishDrop(string drop)
        {
            string line = string.Empty;
            //DropType type;
            using (StringReader reader = new StringReader(drop))
            {
                try
                {
                    line = reader.ReadLine();
                    //dropType = line;
                    //Enum.TryParse(line, out type);
                    //DropType = type;
                    DropType = (DropType)Enum.Parse(typeof(DropType), line);
                    line = reader.ReadLine();
                    DropName = line;
                    line = reader.ReadLine();
                    //dropTime = line;
                    DropTime = DateTime.Parse(line);
                }
                catch (ArgumentException ex)
                {

                    throw;
                }
            }
        }
    }

    public enum DropType
    {
        Character,
        Weapon,
    }

    public enum Type
    {
        Character,
        Weapon,
        Standard,
        Novice
    }

    public enum BannerName
    {

    }
}
