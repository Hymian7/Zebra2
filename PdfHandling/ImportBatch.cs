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

namespace Zebra.PdfHandling
{
    public class ImportBatch : INotifyPropertyChanged
    {
        public FileInfo File { get; set; }
        public PreviewablePdfDocument Document { get; set; }
        public List<ImportAssignment> importAssignments { get; set; }

        public List<ImportCandidate> importCandidates { get; set; }

        public ImportBatch(string filename)
        {
            File = new FileInfo(filename);
            Document = new PreviewablePdfDocument(File.FullName);

            importAssignments = new List<ImportAssignment>();
            importCandidates = new List<ImportCandidate>();

            for (int i = 0; i < Document.PageCount; i++)
            {
                importCandidates.Add(new ImportCandidate(i, Document.Pages[i].Thumbnail));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
