using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Library.Services
{
    public class ArchiveService
    {
        private FileNameService _fileNameService;

        public ArchiveService(FileNameService fileNameService)
        {
            _fileNameService = fileNameService;
        }

        public void PushFile(FileInfo sourceFile, FileInfo destFile, FileImportMode mode = FileImportMode.Copy, bool _override = false)
        {
            //Check if destination file already exists
            if (destFile.Exists && _override == false)
            {
                throw new System.IO.IOException("File already exists and overwriting is disabled.");
            }

            switch (mode)
            {
                case FileImportMode.Copy:
                    //if (!Directory.Exists(Path.FullName + "\\" + sheet.Piece.PieceID)) Directory.CreateDirectory(Path.FullName + "\\" + sheet.Piece.PieceID);
                    sourceFile.CopyTo(destFile.FullName, _override);
                    break;
                case FileImportMode.Move:
                    sourceFile.MoveTo(destFile.FullName, _override);
                    break;
                default:
                    break;
            }
        }

        public void PushFile(string sourceFile, string destFile, FileImportMode mode = FileImportMode.Copy, bool _override = false) => PushFile(new FileInfo(sourceFile), new FileInfo(destFile), mode, _override);

    }
}
