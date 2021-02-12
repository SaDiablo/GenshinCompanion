using System.Windows;
using System.Windows.Controls;
using GenshinWishCalculator.ViewModels;

namespace GenshinWishCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainWindowViewModel;
        }
    }
}
