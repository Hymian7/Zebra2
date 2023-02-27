using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zebra.Library.PdfHandling
{
    public interface IImportCandidateImporter
    {
        Task ImportImportCandidateAsync(ImportCandidate importCandidate);
    }
}