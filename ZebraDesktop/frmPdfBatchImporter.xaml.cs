using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

            context.Piece.Load();
            AllPieces = context.Piece.Local.ToObservableCollection();

            context.Part.Load();
            AllParts = context.Part.Local.ToObservableCollection();

            cbAssPart.ItemsSource = AllParts;
            cbAssPiece.ItemsSource = AllPieces;

        }



        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "PDF Files | *.pdf";
            ofd.Multiselect = false;


            ofd.ShowDialog();

            if (System.IO.File.Exists(ofd.FileName))
            {
                lbThumbnails.ItemsSource = null;

                batch = new ImportBatch(ofd.FileName);

                lbThumbnails.ItemsSource = batch.importCandidates;

            }
        }

        private void lbThumbnails_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (lbThumbnails.SelectedIndex != -1)
            {
                imgPreview.Source = batch.Document.Pages[(lbThumbnails.SelectedItem as ImportCandidate).PageNumber].GetFullScaleImage();

                if ((lbThumbnails.SelectedItem as ImportCandidate).AssignedPart != null)
                {
                    cbAssPart.SelectedItem = (lbThumbnails.SelectedItem as ImportCandidate).AssignedPart;
                    cbAssPiece.SelectedItem = (lbThumbnails.SelectedItem as ImportCandidate).AssignedPiece;
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (lbThumbnails.SelectedItem as ImportCandidate).AssignedPart = (cbAssPart.SelectedItem as Part);
            (lbThumbnails.SelectedItem as ImportCandidate).AssignedPiece = (cbAssPart.SelectedItem as Piece);


            lbThumbnails.SelectedIndex++;
            cbAssPiece.Focus();
        }
    }
}
