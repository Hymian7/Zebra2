using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZebraDesktop
{
    public class ImportBatch
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

    }
}
