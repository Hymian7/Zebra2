using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zebra.PdfHandling;

namespace ZebraDesktop.ViewModels
{
    public class PDFBatchImporterPreviewViewModel : ViewModelBase
    {
        #region Properties
        private ImportBatch _batch;

        public ImportBatch Batch
        {
            get { return _batch; }
            set { _batch = value; ListImportAssignments(); UpdateButtonStatus(); NotifyPropertyChanged(); }
        }

        public DelegateCommand ImportBatchCommand { get; set; }
        #endregion

        #region Constructors

        public PDFBatchImporterPreviewViewModel()
        {
            ImportBatchCommand = new DelegateCommand(executeImportBatchCommand, canExecuteImportBatchCommand);            
        }

        

        public PDFBatchImporterPreviewViewModel(ImportBatch batch) : this()
        {
            Batch = batch;
        }

        #endregion

        #region Methods

        private void ListImportAssignments()
        {
            if (Batch == null) return;

            if (!(Batch.importAssignments == null)) Batch.importAssignments.Clear();

            foreach (var candidate in Batch.importCandidates)
            {
                if (candidate.IsAssigned)
                {
                    Batch.importAssignments.Add(new ImportAssignment(candidate.AssignedPiece, candidate.AssignedPart, new List<int>() { candidate.PageNumber }));
                }
            }
        }

        private bool canExecuteImportBatchCommand(object obj)
        {
            return (Batch?.importAssignments.Count > 0);
        }

        private async void executeImportBatchCommand(object obj)
        {
            try
            {
                await Batch.ImportAllAssignmentsAsync((Application.Current as App).Manager);
            }
            catch (SheetAlreadyExistsException ex)
            {

                if (MessageBox.Show($"The sheet {ex.ExistingSheet.Piece} - {ex.ExistingSheet.Part} already exists. Would you like to override it?", "Sheet already exists", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    await ex.Assignment.ImportAsync((Application.Current as App).Manager, Batch.File, true);
                }
            }
        }

        #endregion


    }
}
