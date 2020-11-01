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
using Zebra.PdfHandling;
using ZebraDesktop.ViewModels;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmPdfBatchImportPreview.xaml
    /// </summary>
    public partial class frmPdfBatchImportPreview : Window
    {
        public frmPdfBatchImportPreview(ImportBatch _batch)
        {
            InitializeComponent();
            this.DataContext= new PDFBatchImporterPreviewViewModel(_batch);

        }
    }
}
