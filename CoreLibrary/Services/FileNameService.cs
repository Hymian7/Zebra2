using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library.Services
{
    public class FileNameService
    {

        private const int FileNameLength = 8;
        private const string Extension = ".pdf";

        /// <summary>
        /// Returns the filename for the PDF file of the provided sheet object including .pdf file extension.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public string GetFileName(Sheet sheet)
        {
            return GetFileName(sheet.SheetID);
        }

        /// <summary>
        /// Returns the filename for the PDF file of the provided sheet id including .pdf file extension
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFileName(int id)
        {
            return id.ToString("").PadLeft(FileNameLength, '0') + ".pdf";
        }

        /// <summary>
        /// Returns the filename for the PDF file of the provided GUID including .pdf file extension
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetFileName(Guid guid)
        {
            return guid.ToString("").PadLeft(FileNameLength, '0') + ".pdf";
        }
    }
}
