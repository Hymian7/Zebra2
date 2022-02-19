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
        public ObservableCollection<ImportCandidate> ImportCandidates { get; set; }

        private ImportCandidateImporter Importer { get; set; }

        public ImportBatch(ImportCandidateImporter importer)
        {
            ImportCandidates = new ObservableCollection<ImportCandidate>();
            Importer = importer;
        }   

        public void AddDocument(string pdfDocument)
        {
            PreviewablePdfDocument doc = new PreviewablePdfDocument(pdfDocument);

            SortedList<int, ImportPage> pages = new SortedList<int, ImportPage>();

            ImportCandidate newCandidate = new ImportCandidate(pdfDocument);

            for (int i = 0; i < doc.PageCount(); i++)
            {
                ImportPage page = new ImportPage(newCandidate, i+1, doc.Pages[i].Thumbnail);
                newCandidate.Pages.Add(page);
            }

            ImportCandidates.Add(newCandidate);

        }


        /// <summary>
        /// Imports all assigned ImportCandidates from the batch.
        /// NOTE: Does not remove them from the batch.
        /// </summary>
        /// <returns></returns>
        public async Task ImportAllAssignedCandidatesAsync()
        {
            foreach (var candidate in ImportCandidates)
            {
                if (candidate.IsAssigned)
                {
                    await Importer.ImportImportCandidate(candidate);
                    //ImportCandidates.Remove(candidate);

                }
            }

        }

        /// <summary>
        /// Imports the given importCandidate and removes it from the ImportBatch.
        /// </summary>
        /// <param name="importCandidate">ImportCandidate to be imported.</param>
        /// <returns></returns>
        public async Task ImportCandidateAsync(ImportCandidate importCandidate)
        {
            await Importer.ImportImportCandidate(importCandidate);
            ImportCandidates.Remove(importCandidate);
        }

     #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

    }
}
