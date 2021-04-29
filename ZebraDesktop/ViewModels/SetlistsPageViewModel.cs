using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;

namespace ZebraDesktop.ViewModels
{
    public class SetlistsPageViewModel : ViewModelBase
    {
        #region Properties

        private Setlist _selectedSetlist;

        public Setlist SelectedSetlist
        {
            get { return _selectedSetlist; }
            set { _selectedSetlist = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<Setlist> _allSetlists;

        public ObservableCollection<Setlist> AllSetlists
        {
            get { return _allSetlists; }
            set { _allSetlists = value; NotifyPropertyChanged(); }
        }

        private CollectionViewSource _filteredSetlists;

        public CollectionViewSource FilteredSetlists
        {
            get { return _filteredSetlists; }
            set { _filteredSetlists = value; NotifyPropertyChanged(); }
        }

        public ICollectionView FilteredPiecesView
        { get { return FilteredSetlists.View; } }

        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; NotifyPropertyChanged(); OnFilterChanged(); }
        }

        private App CurrentApp
        {
            get { return (App)Application.Current; }
        }

        private DelegateCommand _itemDoubleClickCommand;

        public DelegateCommand ItemDoubleClickCommand
        {
            get { return _itemDoubleClickCommand; }
            set { _itemDoubleClickCommand = value; NotifyPropertyChanged(); }
        }


        #endregion

        #region Constructors

        public SetlistsPageViewModel()
        {

            AllSetlists = CurrentApp.Manager.GetAllSetlists();

            FilteredSetlists = new CollectionViewSource();
            FilteredSetlists.Source = AllSetlists;
            FilteredSetlists.Filter += ApplyFilter;

            ItemDoubleClickCommand = new DelegateCommand(ExecuteItemDoubleClick);
        }

        #endregion

        #region Commands
        private void ExecuteItemDoubleClick(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
            return;

            // *********************
            // TODO: Change Filter behaviour

            if (String.IsNullOrEmpty(Filter))
            { e.Accepted = true; }
            else
            {
                Setlist itm = e.Item as Setlist;

                //e.Accepted = itm.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase) || itm.Arranger.Contains(Filter, StringComparison.OrdinalIgnoreCase) || itm.PieceID.ToString().Contains(Filter, StringComparison.OrdinalIgnoreCase);

            }

        }

        private void OnFilterChanged()
        {
            FilteredSetlists.View.Refresh();
        }


        #endregion
    }
}
