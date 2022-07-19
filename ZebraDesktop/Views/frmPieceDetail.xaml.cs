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
using ZebraDesktop.ViewModels;

namespace ZebraDesktop.Views
{
    /// <summary>
    /// Interaktionslogik für frmPieceDetail.xaml
    /// </summary>
    public partial class frmPieceDetail : Window
    {


        public frmPieceDetail(PieceDTO piece)
        {
            InitializeComponent();
            this.DataContext = new PieceDetailViewModel(piece);
        }

        private async void lvSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvSheets.SelectedIndex !=-1)
            {
                dvSheet.Navigate(await ((App)Application.Current).Manager.GetPDFPathAsync((this.DataContext as PieceDetailViewModel).SelectedSheet.SheetID));
            }
        }
    }
}
