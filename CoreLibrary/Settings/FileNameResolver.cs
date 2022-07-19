using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public static class FileNameResolver
    {
        /// <summary>
        /// Returns the filename for the PDF file of the provided sheet object including file extension.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static String GetFileName(Sheet sheet)
        {
            return GetFileName(sheet.SheetID);
        }
        public static String GetFileName(int id)
        {
            return id.ToString("00000000") + ".pdf";
        }
    }
}
