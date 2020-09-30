using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Zebra.Library;

namespace ZebraDesktop.ViewModels
{
    public class PartsPageViewModel : ViewModelBase
    {
        #region Properties

        private Part _selectedPart = null;
        public Part SelectedPart
        {
            get { return _selectedPart; }
            set { _selectedPart = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<Part> _allParts;
        public ObservableCollection<Part> AllParts
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

            CurrentApp.Manager.Context.Part.LoadAsync<Part>();
            AllParts = CurrentApp.Manager.Context.Part.Local.ToObservableCollection();

            FilteredParts = new CollectionViewSource
            {
                Source = AllParts
            };

            FilteredParts.Filter += ApplyFilter;

            ItemDoubleClickCommand = new DelegateCommand(ExecuteItemDoubleClick);

        }

        #endregion

        #region Commands
        private void ExecuteItemDoubleClick(object obj)
        {
            //frmPartDetail frm = new frmPartDetail(SelectedPart);
            //frm.Show();
        }
        #endregion

        #region Methods

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(Filter))
            { e.Accepted = true; }
            else
            {
                Part itm = e.Item as Part;
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
