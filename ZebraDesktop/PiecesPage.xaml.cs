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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zebra.Library;

namespace ZebraDesktop.Forms
{
    /// <summary>
    /// Interaktionslogik für Pieces.xaml
    /// </summary>
    public partial class PiecesPage : Page
    {
        public List<Piece> AllPieces { get; set; }

        public PiecesPage(List<Piece> _pieces)
        {            
            InitializeComponent();
            this.AllPieces = _pieces;

            //Set Binding for Listview
            Binding b = new Binding
            {
                Source = AllPieces,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            lvPieces.SetBinding(ListView.ItemsSourceProperty, b);
            
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvPieces.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(tbFilter.Text))
            { return true; }
            else
            {
                Piece itm = item as Piece;

                if (itm.Arranger == null)
                {
                    return itm.Name.Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase) || itm.PieceID.ToString().Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase);
                }
                else
                    return itm.Name.Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase) || itm.Arranger.Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase) || itm.PieceID.ToString().Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase);


            }

        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvPieces.ItemsSource).Refresh();
        }

        private void page_Pieces_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void lvPieces_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvPieces.SelectedIndex != -1)
            {

                frmPieceDetail pieceDetail = new frmPieceDetail(lvPieces.SelectedItem as Piece);
                pieceDetail.Show();


            }
        }
    }
}
