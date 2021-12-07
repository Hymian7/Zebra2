using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Zebra.Library;
using Zebra.Library.PdfHandling;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmPdfBatchImporter.xaml
    /// </summary>
    public partial class frmPdfBatchImporter : Window
    {

        public frmPdfBatchImporter()
        {
            InitializeComponent();
        }

        // Logic for selecting next ImportCandidate
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Todo: Make this optional/selectable in Preferences
            if (lbDocuments.SelectedIndex < lbDocuments.Items.Count - 1)
            {
                lbDocuments.SelectedItem = lbDocuments.Items[lbDocuments.SelectedIndex + 1];
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
                (this.DataContext as ZebraDesktop.ViewModels.PDFBatchImporterViewModel).SelectedImportPage = (ImportPage)(sender as System.Windows.Controls.ListView).SelectedItem;
            }
        }

        private void lbDocumentThumbnails_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            // Find Descendant

            var d = sender as DependencyObject;
            var scrollViewer = FindDescendant<ScrollViewer>(d);


            if (Keyboard.Modifiers == ModifierKeys.Alt && scrollViewer != null)             
            {
                if (e.Delta < 0)
                {
                    scrollViewer.LineRight();
                    e.Handled = true;
                }
                else
                {
                    scrollViewer.LineLeft();
                    e.Handled = true;
                }
            }            

            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }         


        }

        private T FindDescendant<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
                return null;

            var childCount = VisualTreeHelper.GetChildrenCount(d);

            for (var i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = child as T ?? FindDescendant<T>(child);

                if (result != null)
                    return result;
            }

            return null;
        }

    }

}



