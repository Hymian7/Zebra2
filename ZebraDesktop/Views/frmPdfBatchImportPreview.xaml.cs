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

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmPdfBatchImportPreview.xaml
    /// </summary>
    public partial class frmPdfBatchImportPreview : Window
    {
        public ImportBatch Batch { get; }

        public frmPdfBatchImportPreview(ImportBatch _batch)
        {
            InitializeComponent();

            Batch = _batch;

            if(!(Batch.importAssignments == null)) Batch.importAssignments.Clear();

            foreach (var candidate in Batch.importCandidates)
            {
                if (candidate.IsAssigned)
                {
                    Batch.importAssignments.Add(new ImportAssignment(candidate.AssignedPiece, candidate.AssignedPart, new List<int>(){ candidate.PageNumber }));
                }
            }

            lvPreview.ItemsSource = Batch.importAssignments;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            var manager = app.Manager;

            await Batch.ImportAllAssignments(manager);
            
        }
    }
}
