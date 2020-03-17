using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Library
{
    public class LocalArchive : ZebraArchive
    {
        public DirectoryInfo Path { get; set; }

        public override bool IsConnected => Path.Exists;

        public override FileInfo GetFile(Sheet sheet)
        {
            FileInfo _file = new FileInfo(Path.FullName + "\\" + sheet.Piece.PieceID + "\\" + sheet.SheetID + ".pdf");

            if (_file.Exists)
            {
                return _file;
            }
            else
            {
                throw new FileNotFoundException("File does not exist in Archive.", Path.FullName + "\\" + sheet.Piece.PieceID + "\\" + sheet.SheetID + ".pdf");
            }
        }

        public override void PushFile(FileInfo file, Sheet sheet, FileImportMode mode = FileImportMode.Copy)
        {
            switch (mode)
            {
                case FileImportMode.Copy:
                    if (!Directory.Exists(Path.FullName + "\\" + sheet.Piece.PieceID)) Directory.CreateDirectory(Path.FullName + "\\" + sheet.Piece.PieceID);
                    file.CopyTo(Path.FullName + "\\" + sheet.Piece.PieceID + "\\" + sheet.SheetID + ".pdf", false);
                    break;
                case FileImportMode.Move:
                    file.MoveTo(Path.FullName + "\\" + sheet.Piece.PieceID + "\\" + sheet.SheetID + ".pdf", false);
                    break;
                default:
                    break;
            }
        }

        public LocalArchive(LocalArchiveCredentials credentials)
        {
            this.Path = new DirectoryInfo(credentials.Path);
        }
    }
}
