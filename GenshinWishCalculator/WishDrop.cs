using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GenshinWishCalculator
{
    public class Banner
    {
        private Type bannerType;
        //private List<WishDrop> wishList;
        private BindingList<WishDrop> wishList;
        private int pityLimit;
        //private static DateTime genshinImpactStartTime = new DateTime(2020, 09, 28);
        //public TimeSpan daysSince5Star = DateTime.Now - genshinImpactStartTime;
        
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
        //public List<WishDrop> WishList { get => wishList; set => wishList = value; }
        public BindingList<WishDrop> WishList { get => wishList; set => wishList = value; }
        public int PityLimit { get => pityLimit; set => pityLimit = value; }

        public Banner()
        {
            //wishList = new List<WishDrop>();
            wishList = new BindingList<WishDrop>();
        }
        public Banner(Type _bannerType) : this()
        {
            BannerType = _bannerType;
        }
        public Banner(Type _bannerType, string _genshinInput) : this(_bannerType)
        {
            AddRange(_genshinInput);
        }
        
        [JsonConstructor]
        public Banner(Type bannerType, BindingList<WishDrop> wishList, int pityLimit) =>
            (BannerType, WishList, PityLimit) = (bannerType, wishList, pityLimit);

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
                        //this.wishList.Add(new WishDrop(wholeLine));
                        _wishList.Add(new WishDrop(wholeLine));
                    }

                    wholeLine = string.Empty;
                } while (line != null);
                
            }

            //wishList.Reverse();
            for (int i = 0; i < _wishList.Count; i++)
            {
                wishList.Insert(i, _wishList[i]);
            }
            //if (wishList.Count > 0)
            //{
            //    wishList.InsertRange(0, _wishList);
            //    wishList.
            //}
            //else
            //{
            //    wishList.AddRange(_wishList);
            //}
        }
    }


    public class WishDrop
    {
        private string dropName;
        private DropType dropType;
        private DateTime dropTime;
        private int dropRarity;

        public string DropName { get => dropName; set => dropName = value; }
        public DropType DropType { get => dropType; set => dropType = value; }
        public DateTime DropTime { get => dropTime; set => dropTime = value; }
        public int DropRarity { get => dropRarity; set => dropRarity = value; }

        public WishDrop(DropType _dropType, string _dropName, DateTime _dropTime)
        {
            DropType = _dropType;
            DropName = _dropName;
            DropTime = _dropTime;
        }

        public WishDrop(string drop)
        {
            string line = string.Empty;
            using (StringReader reader = new StringReader(drop))
            {
                try
                {
                    //dropType = line;
                    //Enum.TryParse(line, out type);
                    //DropType = type;

                    //DropType
                    line = reader.ReadLine();
                    DropType = (DropType)Enum.Parse(typeof(DropType), line);

                    //DropName
                    line = reader.ReadLine();
                    DropName = line;

                    //DropRarity
                    /*
                    * System.Text.RegularExpressions.Regex
                    * resultString = Regex.Match(subjectString, @"\d+").Value;
                    * returns a string containing the first occurrence of a number in subjectString.
                    * Int32.Parse(resultString) will then give you the number.
                    */
                    if (int.TryParse(new String(dropName.Where(char.IsDigit).ToArray()), out dropRarity)) { }
                    else { dropRarity = 3; };

                    //DropType
                    line = reader.ReadLine();
                    DropTime = DateTime.Parse(line);
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
