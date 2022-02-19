using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Zebra.Library;
using ZebraDesktop.Views;


namespace ZebraDesktop.ViewModels
{
    public class PiecesPageViewModel : ViewModelBase
    {
        #region Properties

        private Piece _selectedPiece = null;
        public Piece SelectedPiece
        {
            get { return _selectedPiece; }
            set { _selectedPiece = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<Piece> _allPieces;
        public ObservableCollection<Piece> AllPieces
        {
            get { return _allPieces; }
            set { _allPieces = value; NotifyPropertyChanged(); }
        }

        private CollectionViewSource _filteredPieces;
        public CollectionViewSource FilteredPieces
        {
            get { return _filteredPieces; }
            set { _filteredPieces = value; NotifyPropertyChanged(); }
        }

        public ICollectionView FilteredPiecesView
        { get { return FilteredPieces.View; } }


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

        public PiecesPageViewModel()
        {
            CurrentApp = (App)Application.Current;

            CurrentApp.Manager.Context.Piece.LoadAsync<Piece>();
            AllPieces = CurrentApp.Manager.Context.Piece.Local.ToObservableCollection();

            FilteredPieces = new CollectionViewSource();
            FilteredPieces.Source = AllPieces;
            FilteredPieces.Filter += ApplyFilter;

            ItemDoubleClickCommand = new DelegateCommand(ExecuteItemDoubleClick);

        }

        #endregion

        #region Commands
        private void ExecuteItemDoubleClick(object obj)
        {
            frmPieceDetail frm = new frmPieceDetail(SelectedPiece);
            frm.Show();
        }
        #endregion

        #region Methods

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(Filter))
            { e.Accepted = true; }
            else
            {
                Piece itm = e.Item as Piece;

                if (itm.Arranger == null)
                {
                    e.Accepted = itm.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase) || itm.PieceID.ToString().Contains(Filter, StringComparison.OrdinalIgnoreCase);
                }
                else
                    e.Accepted = itm.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase) || itm.Arranger.Contains(Filter, StringComparison.OrdinalIgnoreCase) || itm.PieceID.ToString().Contains(Filter, StringComparison.OrdinalIgnoreCase);

            }

        }

        private void OnFilterChanged()
        {
            FilteredPieces.View.Refresh();
        }

        #endregion
    }
}
