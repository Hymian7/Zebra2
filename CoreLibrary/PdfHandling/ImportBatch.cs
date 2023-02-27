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

namespace Zebra.Library.PdfHandling
{
    public class ImportBatch : ObservableCollection<ImportCandidate>
    {
        public ImportBatch()
        {

        }

    }
}
