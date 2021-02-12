using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zebra.Library;
using PDFtkSharp;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Zebra.PdfHandling
{
    public class ImportBatch : INotifyPropertyChanged
    {
        public FileInfo File { get; set; }

        public ObservableCollection<ImportCandidate> ImportCandidates { get; set; }

        public ImportBatch()
        {
            ImportCandidates = new ObservableCollection<ImportCandidate>();
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

        /// <summary>
        /// Sends generated ImportCandidates back to UI Thread in the ImportBatchProgressReport
        /// Items have to be added to Collection manually in UI Thread
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        //public async Task LoadImportCandidatesAsync(IProgress<ImportBatchProgressReport> progress)
        //{
        //    ImportBatchProgressReport report = new ImportBatchProgressReport();

        //    for (int i = 0; i < Document.PageCount; i++)
        //    {
        //        await Task.Run(() => report.LastImported = (new ImportCandidate(i, Document.Pages[i].Thumbnail)));
        //        report.ImportedCandidates.Add(report.LastImported);
        //        report.Percentage=((int)(i + 1) * 100 / Document.PageCount);
        //        report.GenerateMessage(i + 1, Document.PageCount);

        //        progress.Report(report);

        //    }
        //    report.Percentage = 0;
        //    report.Message = "Import erfolgreich!";
        //    progress.Report(report);
        //}



        public void AddDocument(string pdfDocument)
        {
            PreviewablePdfDocument doc = new PreviewablePdfDocument(pdfDocument);

            SortedList<int, ImportPage> pages = new SortedList<int, ImportPage>();

            ImportCandidate newCandidate = new ImportCandidate(pdfDocument);

            for (int i = 0; i < doc.PageCount; i++)
            {
                ImportPage page = new ImportPage(newCandidate, i+1, doc.Pages[i].Thumbnail);
                newCandidate.Pages.Add(page);
            }

            ImportCandidates.Add(newCandidate);

        }

    }
}
