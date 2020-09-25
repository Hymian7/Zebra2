using Microsoft.EntityFrameworkCore;
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
    /// Interaktionslogik für PartsPage.xaml
    /// </summary>
    public partial class PartsPage : Page
    {
        public PartsPage(ZebraContext context)
        {
            InitializeComponent();

            context.Part.Load();

            //Set Binding for Listview
            Binding b = new Binding
            {
                Source = context.Part.Local.ToObservableCollection(),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            lvParts.SetBinding(ListView.ItemsSourceProperty, b);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvParts.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(tbFilter.Text))
            { return true; }
            else
            {
                Part itm = item as Part;

                if (itm.Name.Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase) || itm.Position.ToString().Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase) || itm.PartID.ToString().Contains(tbFilter.Text, StringComparison.OrdinalIgnoreCase))
                    return true;
                else return false;

            }

        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvParts.ItemsSource).Refresh();
        }
    }
}
