using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zebra.Library;
using PDFtkSharp;
using System.IO;
using Renci.SshNet.Messages;

namespace Zebra.PdfHandling
{
    public class ImportAssignment : INotifyPropertyChanged
    {

        private Piece _piece;

        public Piece Piece
        {
            get { return _piece; }
            set { _piece = value; NotifyPropertyChanged(); }
        }


        private Part _part;
        public Part Part
        {
            get { return _part; }
            set { _part = value; NotifyPropertyChanged(); }
        }


        private List<int> _pages;
        public List<int> Pages
        {
            get { return _pages; }
            set { _pages = value; NotifyPropertyChanged(); }
        }

        private int _progess;
        public int Progress
        {
            get { return _progess; }
            set { _progess = value; NotifyPropertyChanged(); }
        }

        private bool _isImported;
        public bool IsImported
        {
            get { return _isImported; }
            set { _isImported = value; NotifyPropertyChanged(); }
        }


        public ImportAssignment(Piece _piece, Part _part, List<int> _pages)
        {
            Piece = _piece;
            Part = _part;
            Pages = _pages;
            Progress = 0;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task ImportAsync(ZebraDBManager manager, FileInfo file)
        {
            //Check if Sheet is already in Database
            foreach (var sheet in Piece.Sheet)
            {
                Progress = -1;
                if (sheet.Part == Part) throw new SheetAlreadyExistsException(sheet);
                return;
            }

            this.Progress = 25;

            //Create database entry
            var _newsheet = manager.NewSheet(Piece.PieceID, Part.PartID);

            this.Progress = 50;

            //Extract page
            PDFExtractor extractor = new PDFExtractor()
            { 
                InputDocument=file,
                OutputName=_newsheet.SheetID.ToString().PadLeft(8, '0'),
                OutputPath=new DirectoryInfo(manager.ZebraConfig.TempDir)
            };

            this.Progress = 75;

            try
            {
                //Extract the page from the batch
                await extractor.ExtractPagesAsync(Pages.ToArray());
            }
            catch (Exception)
            {
                manager.Context.Remove<Sheet>(_newsheet);
                Progress = -1;
                throw;
            }

            //Push file to archive
            manager.Archive.PushFile(new FileInfo(manager.ZebraConfig.TempDir + $"/{_newsheet.SheetID.ToString().PadLeft(8, '0') + ".pdf"}"), _newsheet, FileImportMode.Copy);

            //Remove temp file
            var oldfile = new FileInfo(manager.ZebraConfig.TempDir + $"/{_newsheet.SheetID.ToString().PadLeft(8, '0') + ".pdf"}");
            oldfile.Delete();

            this.Progress = 100;
            IsImported = true;
        }

    }
}
