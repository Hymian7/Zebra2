using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library;

namespace Zebra.PdfHandling
{
    public class SheetAlreadyExistsException : ZebraImportException
    {
        public Sheet ExistingSheet { get; private set; }

        public SheetAlreadyExistsException() : base()
        { }

        public SheetAlreadyExistsException(string message) : base(message)
        {

        }

        public SheetAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }

        public SheetAlreadyExistsException(Sheet _sheet)
        {
            ExistingSheet = _sheet;
        }

    }
}
