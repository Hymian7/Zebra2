using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Zebra.Library;
using ZebraDesktop.Forms;

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
            
            //TODO: find out why manager is always null
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

        private void mitm_UnloadConfig_Click(object sender, RoutedEventArgs e)
        {
            UnloadConfig();
        }
        private void mitm_NewConfig_Click(object sender, RoutedEventArgs e)
        {
            frmNewConfig dlg = new frmNewConfig();
            dlg.Show();
        }
       
        /// <summary>
        /// Opens Config Selector Dialog and Loads new config
        /// </summary>
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

        /// <summary>
        /// Loads data from the database in the selected configuration
        /// </summary>
        /// <param name="conf"></param>
        private void LoadConfig(ZebraConfig conf)
        {
            currentApp.ZebraConfig = conf;

            //Set Binding for Status strip label
            Binding b = new Binding
            {
                Source = currentApp.ZebraConfig.ConfigName,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            lbl_ConfigName.SetBinding(Label.ContentProperty, b);


            //Load Pieces Page
            PiecesPage pagePieces = new PiecesPage(currentApp.Manager.Context);
            tiPiecesFrame.Content = pagePieces;


            //Load Parts Page
            PartsPage pageParts = new PartsPage(currentApp.Manager.Context);
            tiPartsFrame.Content = pageParts;


            //Add Handlers for Toolbar
            pagePieces.lvPieces.SelectionChanged += AdjustPieceToolbarStatus;


            tcViews.IsEnabled = true;
            //MessageBox.Show($"Konfigruation '{currentApp.ZebraConfig.ConfigName}' erfolgreich geladen");
        }

        private void UnloadConfig()
        {
            currentApp.ZebraConfig = null;

            tiPiecesFrame.Content = null;

            lbl_ConfigName.Content = "Keine Konfiguration geladen.";

            MessageBox.Show("Konfiguration erfolgreich geschlossen", "Konfiguration geschlossen");
        }

        private void tbButtonAddPiece_Click(object sender, RoutedEventArgs e)
        {
            frmNewPiece frm = new frmNewPiece();
            frm.Show();
        }

        /// <summary>
        /// Adjusts state of Piece Toolbar Tray depending on the selection in the lvPieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdjustPieceToolbarStatus(object sender, RoutedEventArgs e)
        {            
            if ((sender as ListView).SelectedIndex >= 0)
            {
                tbButtonEditPiece.IsEnabled = true;
                tbButtonDeletePiece.IsEnabled = true;
            }
            else
            {
                tbButtonEditPiece.IsEnabled = false;
                tbButtonDeletePiece.IsEnabled = false;
            }
        }

        private void tbButtonEditPiece_Click(object sender, RoutedEventArgs e)
        {
            frmPieceDetail frm = new frmPieceDetail((tiPiecesFrame.Content as PiecesPage).lvPieces.SelectedItem as Piece);
            frm.Show();
        }

        private void tbButtonDeletePiece_Click(object sender, RoutedEventArgs e)
        {
            var pc = (tiPiecesFrame.Content as PiecesPage).lvPieces.SelectedItem as Piece;
            switch (MessageBox.Show($"Möchten Sie den Notensatz #{pc.PieceID} - {pc.Name} - {pc.Arranger} und alle zugehörigen Notenblätter wirklich löschen? Die Änderung kann nicht rückgängig gemacht werden!", "Löschen bestätigen", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:

                    foreach (Sheet sht in pc.Sheet)
                    {
                        currentApp.Manager.Context.Remove<Sheet>(sht);
                    }

                    currentApp.Manager.Context.Remove<Piece>(pc);

                    break;
            }
        }

        private void mitm_ImportPdfBatch_Click(object sender, RoutedEventArgs e)
        {
            frmPdfBatchImporter frm = new frmPdfBatchImporter(currentApp.Manager.Context);
            frm.Show();
        }
    }
}
