using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library;

namespace Zebra.PdfHandling
{
    public class SheetAlreadyExistsException : ZebraImportException
    {
        public Sheet ExistingSheet { get; private set; }
        public ImportAssignment Assignment { get; private set; }

        public SheetAlreadyExistsException() : base()
        { }

        public SheetAlreadyExistsException(string message) : base(message)
        {

        }

        public SheetAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }

        public SheetAlreadyExistsException(Sheet _sheet) : base($"Sheet {_sheet.Piece.Name} - {_sheet.Part.Name} already exists.")
        {
            ExistingSheet = _sheet;
        }

        public SheetAlreadyExistsException(Sheet _sheet, ImportAssignment _assignment) : base($"Sheet {_sheet.Piece.Name} - {_sheet.Part.Name} already exists.")
        {
            ExistingSheet = _sheet;
            Assignment = _assignment;
        }

    }
}
