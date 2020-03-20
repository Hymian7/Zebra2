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

namespace ZebraDesktop
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
        }

        private void page_Pieces_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in AllPieces)
            {
                lvPieces.Items.Add(item);
            }
        }
    }
}
