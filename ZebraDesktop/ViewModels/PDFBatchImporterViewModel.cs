using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zebra.Library;
using Zebra.PdfHandling;
using System.Linq;
using System.IO;
using System.Diagnostics;

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

        private ImportPage _selectedImportPage;

        public ImportPage SelectedImportPage
        {
            get { return _selectedImportPage; }
            set
            {
                _selectedImportPage = value;

                // Only change the ImportCandidate when a value is given for the ImportPage
                // Otherwise this would throw an exception, e.g. when page is deleted.
                if(value != null) SelectedImportCandidate = value.ImportCandidate;
                
                NotifyPropertyChanged(); UpdateButtonStatus();
            }
        }

        private ImportCandidate _selectedImportCandidate;

        public ImportCandidate SelectedImportCandidate
        {
            get { return _selectedImportCandidate; }
            set
            {
                _selectedImportCandidate = value;
                if (value != null && SelectedImportPage != null && SelectedImportPage.ImportCandidate != value)
                {
                    SelectedImportPage = value.Pages.FirstOrDefault();
                }
                
                NotifyPropertyChanged(); UpdateButtonStatus();
            }
        }


        private ImportBatchProgressReport _report;

        public ImportBatchProgressReport ImportReport
        {
            get { return _report; }
            set { _report = value; NotifyPropertyChanged(); }
        }

        private bool _takeoverAssignedPiece;

        public bool TakeoverAssignedPiece
        {
            get { return _takeoverAssignedPiece; }
            set { _takeoverAssignedPiece = value; NotifyPropertyChanged(); }
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

        private DelegateCommand _deleteSelectedImportCandidateCommand;

        public DelegateCommand DeleteSelectedImportCandidateCommand
        {
            get { return _deleteSelectedImportCandidateCommand; }
            set { _deleteSelectedImportCandidateCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _deleteSelectedImportPageCommand;

        public DelegateCommand DeleteSelectedImportPageCommand
        {
            get { return _deleteSelectedImportPageCommand; }
            set { _deleteSelectedImportPageCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _openDocumentInExplorerCommand;

        public DelegateCommand OpenDocumentInExplorerCommand
        {
            get { return _openDocumentInExplorerCommand; }
            set { _openDocumentInExplorerCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _splitImportCandidateOnPage;

        public DelegateCommand SplitImportCandidateOnPage
        {
            get { return _splitImportCandidateOnPage; }
            set { _splitImportCandidateOnPage = value; NotifyPropertyChanged(); }
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

            DeleteSelectedImportCandidateCommand = new DelegateCommand(executeDelteSelectedImportCandidateCommand, canExecuteDeleteSelectedImportCandidateCommand);
            DeleteSelectedImportPageCommand = new DelegateCommand(executeDeleteSelectedImportPageCommand, canExecuteDeleteSelectedImportPageCommand);
            OpenDocumentInExplorerCommand = new DelegateCommand(executeOpenDocumentInExplorerCommand, canExecuteOpenDocumentInExplorerCommand);
            SplitImportCandidateOnPage = new DelegateCommand(executeSplitImportCandidateOnPage, canExecuteSplitImportCandidateOnPage);

            TakeoverAssignedPiece = true;
        }

        

        #endregion

        #region Commands
        private async void executeOpenFileCommand(object obj)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "PDF Files | *.pdf";
            ofd.Multiselect = true;


            ofd.ShowDialog();

            if (System.IO.File.Exists(ofd.FileName))
            {

                if(Batch == null) Batch = new ImportBatch();

                foreach (var file in ofd.FileNames)
                {

                    Batch.AddDocument(file);

                }
                

            }
        }

        private bool canExecuteOpenFileCommand(object obj)
        {
            return true;
        }

        private void executeImportCommand(object obj)
        {
            
            //Batch.ImportAssignments.Add(new ImportAssignment(item.AssignedPiece, item.AssignedPart, new List<int>() { item.PageNumber }));
            throw new NotImplementedException("Needs reimplementation");
                
        }

        private bool canExecuteImportCommand(object obj)
        {
            return SelectedImportCandidate.IsAssigned;
        }

        private void executeAssignCommand(object obj)
        {
            UpdateButtonStatus();
        }

        private bool canExecuteAssignCommand(object obj)
        {
            //return (SelectedItem?.AssignedPart != null && SelectedItem?.AssignedPiece != null);
            return true;
        }

        private bool canExecuteDeleteSelectedImportPageCommand(object obj)
        {
            // Page can only be deleted if the selected ImportCandidate has 2 or more pages.
            // In case of 1 page only, the document has to be deleted instead.
            return SelectedImportPage != null && SelectedImportPage.ImportCandidate.Pages.Count >1;
        }

        private bool canExecuteDeleteSelectedImportCandidateCommand(object obj)
        {
            return SelectedImportCandidate != null;
        }

        private void executeDelteSelectedImportCandidateCommand(object obj)
        {
            Batch.ImportCandidates.Remove(SelectedImportCandidate);
        }

        private void executeDeleteSelectedImportPageCommand(object obj)
        {
            // Check if something went wrong with the selection
            if (SelectedImportPage.ImportCandidate != SelectedImportCandidate) throw new Exception("ImportCandidate does not match ImportPage");
            
            SelectedImportPage.ImportCandidate.Pages.Remove(SelectedImportPage);

        }

        private bool canExecuteOpenDocumentInExplorerCommand(object obj)
        {
            return SelectedImportCandidate != null;
        }

        private void executeOpenDocumentInExplorerCommand(object obj)
        {
            Process.Start("explorer.exe", $"/select, { SelectedImportCandidate.DocumentPath }");
        }

        private bool canExecuteSplitImportCandidateOnPage(object obj)
        {
            if (SelectedImportCandidate == null) return false;
            if (SelectedImportPage == null) return false;

            // Document cannot be split if ImportCandidate only has 1 page
            if (SelectedImportPage.ImportCandidate.Pages.Count < 2) return false;

            // Document cannot be split on first page
            if (SelectedImportPage.ImportCandidate.Pages.IndexOf(SelectedImportPage) == 0) return false;

            return true;
        }

        private void executeSplitImportCandidateOnPage(object obj)
        {
            var pageIndex = SelectedImportPage.ImportCandidate.Pages.IndexOf(SelectedImportPage);
            var numPages = SelectedImportPage.ImportCandidate.Pages.Count;
            var importCandidateIndex = Batch.ImportCandidates.IndexOf(SelectedImportCandidate);

            var newImportCandidate = new ImportCandidate(SelectedImportCandidate.DocumentPath);
            var oldImportCandidate = SelectedImportPage.ImportCandidate;

            for (int i = pageIndex; i < numPages; i++)
            {                
                // Always same pageIndex, because page gets removed and following page indices decrease
                var page = oldImportCandidate.Pages[pageIndex];

                // Set new parent for the page and add to new Import Candidate
                page.ImportCandidate = newImportCandidate;
                newImportCandidate.Pages.Add(page);

                // Remove page from old ImportCandidate
                oldImportCandidate.Pages.Remove(page);

            }

            if (TakeoverAssignedPiece == true)
            {
                newImportCandidate.AssignedPiece = oldImportCandidate.AssignedPiece;
            }

            Batch.ImportCandidates.Add(newImportCandidate);
            Batch.ImportCandidates.Move(Batch.ImportCandidates.IndexOf(newImportCandidate), importCandidateIndex + 1);


        }

        #endregion


    }
}
