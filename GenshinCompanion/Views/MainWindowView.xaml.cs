using System;
using System.Windows;
using GenshinWishCalculator.ViewModels;

namespace GenshinWishCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();

        public MainWindowView()
        {
            InitializeComponent();
            DataContext = _mainWindowViewModel;
#if NET472
            InitializeNotifyIcon(); //NotMVVMCompliant
#endif
        }

        #region NotMVVMCompliant
#if NET472
        private bool _visible = true;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuItem;

        private void InitializeNotifyIcon()
        {

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            contextMenu = new System.Windows.Forms.ContextMenu();
            menuItem = new System.Windows.Forms.MenuItem();

            // Initialize menuItem
            menuItem.Index = 0;
            menuItem.Text = "E&xit";
            menuItem.Click += new EventHandler(MenuItem_Click);

            // Initialize contextMenu
            contextMenu.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { menuItem });

            notifyIcon.ContextMenu = contextMenu;

            Uri iconUri = new Uri("pack://application:,,,/Assets/Item_Primogem_256.ico", UriKind.Absolute);
            notifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(iconUri).Stream);
            notifyIcon.Visible = true;
            notifyIcon.Click += NotifyIcon_Click;

        }

        void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (!_visible)
            {
                Visibility = Visibility.Visible;
                WindowState = WindowState.Normal;
                Topmost = true;
                _visible = !_visible;
                Topmost = false;
            }
            else
            {
                Visibility = Visibility.Hidden;
                _visible = !_visible;
            }
        }

        /// <summary>
        /// Close the form, which closes the application.
        /// </summary>
        private void MenuItem_Click(object Sender, EventArgs e) => Close();
#endif
#endregion
    }
}
