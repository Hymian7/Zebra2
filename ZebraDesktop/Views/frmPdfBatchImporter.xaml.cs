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
        
        public frmPdfBatchImporter(ZebraContext context)
        {
            InitializeComponent();
        }

        // Logic for selecting next ImportCandidate
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Todo: Make this optional/selectable in Preferences
            if (lbThumbnails.SelectedIndex < lbThumbnails.Items.Count - 1)
            {
                lbThumbnails.SelectedItem = lbThumbnails.Items[lbThumbnails.SelectedIndex+1];
            }
            else
            {
                //Todo: Find Bug with FilterableComboBox
                //Filterable Combobox does not Update when Selected Index is set to 0
                //lbThumbnails.SelectedItem = lbThumbnails.Items[0];
            }

            cbAssPiece.Focus();
        }

    }
}
