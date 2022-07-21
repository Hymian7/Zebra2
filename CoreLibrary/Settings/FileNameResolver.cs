using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public static class FileNameResolver
    {
        /// <summary>
        /// Returns the filename for the PDF file of the provided sheet object including .pdf file extension.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static String GetFileName(Sheet sheet)
        {
            return GetFileName(sheet.SheetID);
        }

        /// <summary>
        /// Returns the filename for the PDF file of the provided sheet id including .pdf file extension
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String GetFileName(int id)
        {
            return id.ToString("").PadLeft(8, '0') + ".pdf";
        }
    }
}
