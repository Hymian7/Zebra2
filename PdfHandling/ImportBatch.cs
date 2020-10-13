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
        public PreviewablePdfDocument Document { get; set; }
        public ObservableCollection<ImportAssignment> importAssignments { get; set; }

        public ObservableCollection<ImportCandidate> importCandidates { get; set; }

        public ImportBatch(string filename)
        {
            File = new FileInfo(filename);
            Document = new PreviewablePdfDocument(File.FullName);

            importAssignments = new ObservableCollection<ImportAssignment>();
            importCandidates = new ObservableCollection<ImportCandidate>();

            //LoadImportCandidates();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sends generated ImportCandidates back to UI Thread in the ImportBatchProgressReport
        /// Items have to be added to Collection manually in UI Thread
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async Task LoadImportCandidatesAsync(IProgress<ImportBatchProgressReport> progress)
        {
            ImportBatchProgressReport report = new ImportBatchProgressReport();

            for (int i = 0; i < Document.PageCount; i++)
            {
                await Task.Run(() => report.LastImported = (new ImportCandidate(i, Document.Pages[i].Thumbnail)));
                report.ImportedCandidates.Add(report.LastImported);
                report.Percentage=((int)(i + 1) * 100 / Document.PageCount);
                report.GenerateMessage(i + 1, Document.PageCount);

                progress.Report(report);

            }
            report.Percentage = 0;
            report.Message = "Import erfolgreich!";
            progress.Report(report);
        }

        public void LoadImportCandidates()
        {
            for (int i = 0; i < Document.PageCount; i++)
            {
                importCandidates.Add(new ImportCandidate(i, Document.Pages[i].Thumbnail));
            }
        }

        public async Task ImportAllAssignments(ZebraDBManager manager)
        {
            var tasks = new List<Task>();

            foreach (var item in importAssignments)
            {
                tasks.Add(item.ImportAsync(manager, File));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (SheetAlreadyExistsException ex)
            {
                Debug.Print($"{ex.ExistingSheet.Part.Name} - {ex.ExistingSheet.Piece.Name} wurde nicht importiert, da das Sheet schon existiert hat.");
            }

            catch (Exception ex)
            { Debug.Print(ex.Message); }

        }

    }
}
