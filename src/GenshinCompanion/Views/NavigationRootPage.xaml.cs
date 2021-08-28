using System.Windows.Controls;

namespace GenshinCompanion.Views
{
    /// <summary>
    /// Interaction logic for NavigationRootPage.xaml
    /// </summary>
    public partial class NavigationRootPage : Page
    {
        public NavigationRootPage()
        {
            InitializeComponent();
            NavView.SelectedItem = NavView.MenuItems[0];
        }
    }
}