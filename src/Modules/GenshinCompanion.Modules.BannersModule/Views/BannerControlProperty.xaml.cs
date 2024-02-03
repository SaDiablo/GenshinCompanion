using System.Windows;
using System.Windows.Controls;
using GenshinCompanion.Modules.BannersModule.Models;

namespace GenshinCompanion.Modules.BannersModule.Views
{
    /// <summary>
    /// Properties for BannerControl.xaml
    /// </summary>
    public partial class BannerControl : UserControl
    {
        public Banner Banner
        {
            get => GetValue(BannerProperty) as Banner;
            set => SetValue(BannerProperty, value);
        }

        public static readonly DependencyProperty BannerProperty = DependencyProperty.Register(
            "Banner",
            typeof(Banner),
            typeof(BannerControl));
    }
}