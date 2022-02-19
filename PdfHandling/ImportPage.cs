using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.PdfHandling
{
    public class ImportPage
    {
        public int PageNumber { get; private set; }

        public System.Drawing.Image Thumbnail { get; private set; }

        public ImportCandidate ImportCandidate { get; set; }

        public ImportPage(ImportCandidate parent, int pageNumber, System.Drawing.Image thumbnail)
        {
            PageNumber = pageNumber;
            Thumbnail = thumbnail;
            ImportCandidate = parent;
        }

    }
}
