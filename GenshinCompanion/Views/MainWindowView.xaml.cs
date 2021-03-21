using System;
using System.Windows;
using System.Windows.Controls;
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

            InitializeNotifyIcon(); //NotMVVMCompliant
        }

        #region NotMVVMCompliant
        private bool _visible = true;
#if NET472
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuItem;
        private System.ComponentModel.IContainer components;
#endif
        private void InitializeNotifyIcon()
        {
#if NET472
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem = new System.Windows.Forms.MenuItem();

            // Initialize menuItem
            this.menuItem.Index = 0;
            this.menuItem.Text = "E&xit";
            this.menuItem.Click += new EventHandler(this.menuItem_Click);

            // Initialize contextMenu
            this.contextMenu.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem });

            notifyIcon.ContextMenu = this.contextMenu;

            Uri iconUri = new Uri("pack://application:,,,/Assets/Item_Primogem_256.ico", UriKind.Absolute);
            notifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(iconUri).Stream);
            notifyIcon.Visible = true;
            notifyIcon.Click += nIcon_Click;
#endif
        }

        void nIcon_Click(object sender, EventArgs e)
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

        private void menuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
        
#endregion

    }
}
