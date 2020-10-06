using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Zebra.Library;
using Zebra.PdfHandling;

namespace ZebraDesktop.ViewModels
{
    public class PieceDetailViewModel : ViewModelBase
    {
        #region Properties

        private Piece _currentPiece;

        public Piece CurrentPiece
        {
            get { return _currentPiece; }
            set { _currentPiece = value; NotifyPropertyChanged(); }
        }

        public ZebraDBManager Manager
        { get { return ((Application.Current) as App).Manager; } }

        private Sheet _selectedSheet;

        public Sheet SelectedSheet
        {
            get { return _selectedSheet; }
            set
            {
                _selectedSheet = value;
                if (value != null)
                {
                    CurrentSheetDocumentPath = SelectedSheet.DocumentPath(Manager);
                    CurrentSheetDocument = new PreviewablePdfDocument(CurrentSheetDocumentPath);
                }
                NotifyPropertyChanged();
            }
        }

        private PreviewablePdfDocument _currentSheetDocument;

        public PreviewablePdfDocument CurrentSheetDocument
        {
            get { return _currentSheetDocument; }
            set { _currentSheetDocument = value; NotifyPropertyChanged(); }
        }

        private String _currentSheetDocumentPath;

        public String CurrentSheetDocumentPath
        {
            get { return _currentSheetDocumentPath; }
            set { _currentSheetDocumentPath = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region Constructors

        public PieceDetailViewModel(Piece piece)
        {
            CurrentPiece = piece;
        }

        public PieceDetailViewModel()
        {
        }

        #endregion

        #region Commands

        #endregion
    }
}
