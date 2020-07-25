using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Zebra.Library;

namespace ZebraDesktop
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

            currentApp = (App)Application.Current;
            manager = currentApp.Manager;
        }

        private void frmMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentApp.ZebraConfig == null)
            {
                SelectConfig();
            }
        }

        private void mitm_Exit_Click(object sender, RoutedEventArgs e) => Close();

        private void mitm_LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            SelectConfig();
        }

        private void SelectConfig()
        {
            ConfigSelector frmConfigSelector = new ConfigSelector();
            frmConfigSelector.Owner = this;

            frmConfigSelector.ShowDialog();

            if (!(currentApp.ZebraConfig == null))
            {
                UnloadConfig();
            }

            if (frmConfigSelector.DialogResult == true)
            {
                LoadConfig(frmConfigSelector.SelectedConfiguration);
            }
        }

        private void LoadConfig(ZebraConfig conf)
        {
            currentApp.ZebraConfig = conf;

            Binding b = new Binding
            {
                Source = currentApp.ZebraConfig.ConfigName,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            lbl_ConfigName.SetBinding(Label.ContentProperty, b);


            //Load Pieces Page
            PiecesPage pagePieces = new PiecesPage(currentApp.Manager.GetAllPieces());
            tiPiecesFrame.Content = pagePieces;
            tcViews.IsEnabled = true;

            MessageBox.Show($"Konfigruation '{currentApp.ZebraConfig.ConfigName}' erfolgreich geladen");
        }

        private void mitm_UnloadConfig_Click(object sender, RoutedEventArgs e)
        {
            UnloadConfig();
        }

        private void UnloadConfig()
        {
            currentApp.ZebraConfig = null;

            tiPiecesFrame.Content = null;

            lbl_ConfigName.Content = "Keine Konfiguration geladen.";

            MessageBox.Show("Konfiguration erfolgreich geschlossen", "Konfiguration geschlossen");
        }

        private void mitm_NewConfig_Click(object sender, RoutedEventArgs e)
        {
            frmNewConfig dlg = new frmNewConfig();
            dlg.Show();
        }
    }
}
