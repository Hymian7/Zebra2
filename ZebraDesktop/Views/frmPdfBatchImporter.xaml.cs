using System.Collections.Generic;
using System.Windows;
using Zebra.Library;
using Zebra.PdfHandling;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmPdfBatchImporter.xaml
    /// </summary>
    public partial class frmPdfBatchImporter : Window
    {

        public frmPdfBatchImporter(ZebraContext context)
        {
            InitializeComponent();
        }

        // Logic for selecting next ImportCandidate
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Todo: Make this optional/selectable in Preferences
            if (lbDocuments.SelectedIndex < lbDocuments.Items.Count - 1)
            {
                lbDocuments.SelectedItem = lbDocuments.Items[lbDocuments.SelectedIndex+1];
            }
            else
            {
                //Todo: Find Bug with FilterableComboBox
                //Filterable Combobox does not Update when Selected Index is set to 0
                //lbThumbnails.SelectedItem = lbThumbnails.Items[0];
            }

            cbAssPiece.Focus();
        }

        private void lbDocumentThumbnails_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.ListView).SelectedItem != null)
            { 
                (this.DataContext as ZebraDesktop.ViewModels.PDFBatchImporterViewModel).SelectedImportPage = (KeyValuePair<int, ImportPage>)(sender as System.Windows.Controls.ListView).SelectedItem; 
            }
        }
    }
}
