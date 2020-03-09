using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Zebra.Library;

namespace Zebra.Library
{
    public class ArchiveManager
    {
        #region Properties

        public DirectoryInfo ArchivePath { get; set; }

        #endregion

        #region Constructor

        public ArchiveManager()
        {
            ArchivePath = new DirectoryInfo(@"\\NASFM\Lukas\zebra\");
        }

        #endregion

        public bool StoreSheet(FileInfo pdf, Sheet sheet)
        {
            try
            {
                File.Copy(pdf.FullName, ArchivePath.FullName + sheet.SheetID + ".pdf");
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            } 
            
        }

        public bool StoreSheet(String filepath, Sheet sheet) => StoreSheet(new FileInfo(filepath), sheet);

        public void OpenSheet(Sheet sheet)
        {
            Process.Start(ArchivePath.FullName + sheet.SheetID + ".pdf");
        }

    }
}
