using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zebra.Library;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmPieceDetail.xaml
    /// </summary>
    public partial class frmPieceDetail : Window
    {
        public Piece CurrentPiece { get; private set; }

        private App currentApp;
        private ZebraDBManager manager;


        public frmPieceDetail(Piece _piece)
        {
            InitializeComponent();

            currentApp = (App)Application.Current;
            manager = currentApp.Manager;

            CurrentPiece = manager.GetPieceByID(_piece.PieceID);

            this.DataContext = CurrentPiece;

            lblHeader.Content = "Details: " + CurrentPiece.Name;

            //Set Binding for the Sheets
            Binding bdgSheets = new Binding { 
            Source = CurrentPiece.Sheet,
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.Default
            };

            lvSheets.SetBinding(ListView.ItemsSourceProperty, bdgSheets);

        }

        private void lvSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvSheets.SelectedIndex !=-1)
            {
                dvSheet.Navigate((lvSheets.SelectedItem as Sheet).DocumentPath(((App)Application.Current).Manager));
            }
        }
    }
}
