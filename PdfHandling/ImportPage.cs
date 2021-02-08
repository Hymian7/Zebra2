using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.PdfHandling
{
    public class ImportPage
    {
        public string DocumentPath { get; private set; }
        public int PageNumber { get; private set; }

        public System.Drawing.Image Thumbnail { get; private set; }

        public ImportPage(string documentPath, int pageNumber, System.Drawing.Image thumbnail)
        {
            DocumentPath = documentPath;
            PageNumber = pageNumber;
            Thumbnail = thumbnail;
        }

    }
}
