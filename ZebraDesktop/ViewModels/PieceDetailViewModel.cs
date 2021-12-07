using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Zebra.Library;
using Zebra.Library.PdfHandling;

namespace ZebraDesktop.ViewModels
{
    public class PieceDetailViewModel : ViewModelBase
    {
        #region Properties

        private PieceDTO _currentPiece;

        public PieceDTO CurrentPiece
        {
            get { return _currentPiece; }
            set { _currentPiece = value; NotifyPropertyChanged(); }
        }

        public IZebraDBManager Manager
        { get { return ((Application.Current) as App).Manager; } }

        private SheetDTO _selectedSheet;

        public SheetDTO SelectedSheet
        {
            get { return _selectedSheet; }
            set
            {
                _selectedSheet = value;
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

        public PieceDetailViewModel(PieceDTO piece)
        {
            CurrentPiece = piece;
        }

        public PieceDetailViewModel()
        {
            PieceDTO testPiece = new PieceDTO() { Name = "Quak", Arranger = "Frosch"};
            CurrentPiece = testPiece;
        }

        #endregion

        #region Commands

        #endregion

        #region Methods

        #endregion
    }
}
