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

        private PieceDTO _selectedPiece = null;
        public PieceDTO SelectedPiece
        {
            get { return _selectedPiece; }
            set { _selectedPiece = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<PieceDTO> _allPieces;
        public ObservableCollection<PieceDTO> AllPieces
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

        private App CurrentApp
        {
            get { return (App)Application.Current; }
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

            //CurrentApp.Manager.Context.Piece.LoadAsync<Piece>();
            //AllPieces = CurrentApp.Manager.Context.Piece.Local.ToObservableCollection();

            AllPieces = new ObservableCollection<PieceDTO>();

            UpdateAsync();

            FilteredPieces = new CollectionViewSource();
            FilteredPieces.Source = AllPieces;
            FilteredPieces.Filter += ApplyFilter;

            ItemDoubleClickCommand = new DelegateCommand(ExecuteItemDoubleClick);

        }

        #endregion

        #region Commands
        private async void ExecuteItemDoubleClick(object obj)
        {
            
            frmPieceDetail frm = new frmPieceDetail(await CurrentApp.Manager.GetPieceAsync(SelectedPiece.PieceID));
            frm.Show();
        }
        #endregion

        #region Methods

        public async Task UpdateAsync()
        {
            var collection = await CurrentApp.Manager.GetAllPiecesAsync();
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>

            {
                AllPieces.Clear();
                foreach (var item in collection)
                {
                    AllPieces.Add(item);
                }

            })); ;
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(Filter))
            { e.Accepted = true; }
            else
            {
                PieceDTO itm = e.Item as PieceDTO;

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
