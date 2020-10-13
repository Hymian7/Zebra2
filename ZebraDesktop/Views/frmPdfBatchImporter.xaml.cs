using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PdfiumLight;
using Zebra.Library;
using Zebra.PdfHandling;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmPdfBatchImporter.xaml
    /// </summary>
    public partial class frmPdfBatchImporter : Window
    {
        private readonly ZebraContext context;

        private ObservableCollection<Piece> AllPieces;
        private ObservableCollection<Part> AllParts;

        private ImportBatch batch;

        public frmPdfBatchImporter(ZebraContext context)
        {
            InitializeComponent();

        }

        private void lbThumbnails_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //if (lbThumbnails.SelectedIndex != -1)
            //{
            //    imgPreview.Source = batch.Document.Pages[(lbThumbnails.SelectedItem as ImportCandidate).PageNumber].RenderedPage.ConvertToImageSource();

            //    if ((lbThumbnails.SelectedItem as ImportCandidate).AssignedPart != null)
            //    {
            //        cbAssPart.SelectedItem = (lbThumbnails.SelectedItem as ImportCandidate).AssignedPart;
            //        //cbAssPart.Text = (lbThumbnails.SelectedItem as ImportCandidate).AssignedPart.Name;
            //        cbAssPiece.SelectedItem = (lbThumbnails.SelectedItem as ImportCandidate).AssignedPiece;
            //    }
            //    else
            //    {
            //        if(lbThumbnails.SelectedIndex >0) cbAssPiece.SelectedItem = (lbThumbnails.Items[lbThumbnails.SelectedIndex-1] as ImportCandidate).AssignedPart;
            //        cbAssPart.SelectedItem = null;
            //    }

            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Todo: Make this optional/selectable in Preferences
            if (lbThumbnails.SelectedIndex < lbThumbnails.Items.Count - 1)
            {
                lbThumbnails.SelectedItem = lbThumbnails.Items[lbThumbnails.SelectedIndex+1];
            }
            else
            {
                lbThumbnails.SelectedItem = lbThumbnails.Items[0];
            }

            cbAssPiece.Focus();
        }

    }
}
