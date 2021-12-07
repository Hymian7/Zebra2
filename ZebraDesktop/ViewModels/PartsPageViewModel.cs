using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Zebra.Library;
using ZebraDesktop.Views;
using System.Linq;
using System.Threading.Tasks;

namespace ZebraDesktop.ViewModels
{
    public class PartsPageViewModel : ViewModelBase
    {
        #region Properties

        private PartDTO _selectedPart = null;
        public PartDTO SelectedPart
        {
            get { return _selectedPart; }
            set { _selectedPart = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<PartDTO> _allParts;
        public ObservableCollection<PartDTO> AllParts
        {
            get { return _allParts; }
            set { _allParts = value; NotifyPropertyChanged(); }
        }

        private CollectionViewSource _filteredParts;
        public CollectionViewSource FilteredParts
        {
            get { return _filteredParts; }
            set { _filteredParts = value; NotifyPropertyChanged(); }
        }

        private App _currentApp;
        private App CurrentApp
        {
            get { return _currentApp; }
            set { _currentApp = value; NotifyPropertyChanged(); }
        }

        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; NotifyPropertyChanged(); OnFilterChanged(); }
        }


        private DelegateCommand _itemDoubleClickCommand;

        public DelegateCommand ItemDoubleClickCommand
        {
            get { return _itemDoubleClickCommand; }
            set { _itemDoubleClickCommand = value; NotifyPropertyChanged(); }
        }


        #endregion

        #region Constructors

        public PartsPageViewModel()
        {
            CurrentApp = (Application.Current as App);

            AllParts = new ObservableCollection<PartDTO>();

            UpdateAsync();
            

            FilteredParts = new CollectionViewSource
            {
                Source = AllParts
            };

            FilteredParts.SortDescriptions.Add(new SortDescription("Position", ListSortDirection.Ascending));
            FilteredParts.Filter += ApplyFilter;

            ItemDoubleClickCommand = new DelegateCommand(ExecuteItemDoubleClick);

        }

        #endregion

        #region Commands
        private async void ExecuteItemDoubleClick(object obj)
        {
            
            frmPartDetail frm = new frmPartDetail(await CurrentApp.Manager.GetPartAsync(SelectedPart.PartID));
            frm.Show();
        }
        #endregion

        #region Methods

        public async Task UpdateAsync()
        {
            var collection = await CurrentApp.Manager.GetAllPartsAsync();
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>

            {
                AllParts.Clear();
                foreach (var item in collection)
                {
                    AllParts.Add(item);
                }

            })); ;
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(Filter))
            { e.Accepted = true; }
            else
            {
                PartDTO itm = e.Item as PartDTO;
                e.Accepted = itm.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase) || itm.PartID.ToString().Contains(Filter, StringComparison.OrdinalIgnoreCase);
            }

        }

        private void OnFilterChanged()
        {
            FilteredParts.View.Refresh();
        }

        #endregion

    }
}
