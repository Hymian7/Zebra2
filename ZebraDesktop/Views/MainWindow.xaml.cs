using System.Windows;
using Zebra.Library;

namespace ZebraDesktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly App currentApp;
        private readonly ZebraDBManager manager;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
