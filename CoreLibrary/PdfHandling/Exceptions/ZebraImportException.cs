using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library;

namespace Zebra.Library.PdfHandling
{
    public class ZebraImportException : Exception
    {
        

        public ZebraImportException() : base()
        { }

        public ZebraImportException(string message) : base(message)
        {

        }

        public ZebraImportException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
