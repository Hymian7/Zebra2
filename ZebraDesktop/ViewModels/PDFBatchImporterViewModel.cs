using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using Zebra.Library;
using Zebra.PdfHandling;

namespace ZebraDesktop.ViewModels
{
    public class PDFBatchImporterViewModel : ViewModelBase
    {
        #region Properties

        private ZebraDBManager Manager { get { return (Application.Current as App).Manager; } }

        private ObservableCollection<Piece> _allPieces;
        public ObservableCollection<Piece> AllPieces
        {
            get { return _allPieces; }
            set { _allPieces = value; NotifyPropertyChanged(); }
        }
        
        private ObservableCollection<Part> _allParts;
        public ObservableCollection<Part> AllParts
        {
            get { return _allParts; }
            set { _allParts = value; NotifyPropertyChanged(); }
        }

        private ImportBatch _batch;

        public ImportBatch Batch
        {
            get { return _batch; }
            set { _batch = value; NotifyPropertyChanged();}
        }

        private ImportCandidate _selectedItem;

        public ImportCandidate SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyPropertyChanged(); UpdateButtonStatus(); }
        }

        private DelegateCommand _openFileCommand;

        public DelegateCommand OpenFileCommand
        {
            get { return _openFileCommand; }
            set { _openFileCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _importCommand;

        public DelegateCommand ImportCommand
        {
            get { return _importCommand; }
            set { _importCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _assignCommand;

        public DelegateCommand AssignCommand
        {
            get { return _assignCommand; }
            set { _assignCommand = value; NotifyPropertyChanged(); }
        }




        #endregion

        #region Constructors

        public PDFBatchImporterViewModel()
        {
            Manager.Context.Piece.Load();
            AllPieces = Manager.Context.Piece.Local.ToObservableCollection();

            Manager.Context.Part.Load();
            AllParts = Manager.Context.Part.Local.ToObservableCollection();

            OpenFileCommand = new DelegateCommand(executeOpenFileCommand, canExecuteOpenFileCommand);
            ImportCommand = new DelegateCommand(executeImportCommand, canExecuteImportCommand);
            AssignCommand = new DelegateCommand(executeAssignCommand, canExecuteAssignCommand);

        }


        #endregion

        #region Commands
        private void executeOpenFileCommand(object obj)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "PDF Files | *.pdf";
            ofd.Multiselect = false;


            ofd.ShowDialog();

            if (System.IO.File.Exists(ofd.FileName))
            {

                Batch = new ImportBatch(ofd.FileName);
                Batch.PropertyChanged += base.UpdateButtonStatus;

            }
        }

        private bool canExecuteOpenFileCommand(object obj)
        {
            return true;
        }

        private void executeImportCommand(object obj)
        {
            var frm = new frmPdfBatchImportPreview(Batch);
            frm.ShowDialog();
        }

        private bool canExecuteImportCommand(object obj)
        {
            return Batch != null ? (Batch.importAssignments.Count > 0) : false ;
        }

        private void executeAssignCommand(object obj)
        {
 
        }

        private bool canExecuteAssignCommand(object obj)
        {
            //return (SelectedItem?.AssignedPart != null && SelectedItem?.AssignedPiece != null);
            return true;
        }

        #endregion


    }
}
