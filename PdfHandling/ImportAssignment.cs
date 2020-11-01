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

        public async Task ImportAsync(ZebraDBManager manager, FileInfo file, bool _override = false)
        {
            bool alreadyExists = false;
            Sheet existingSheet=null;

            //Check if Sheet is already in Database
            foreach (var sheet in Piece.Sheet)
            {
                Progress = -1;
                if (sheet.Part == Part)
                {

                    //If no overwriting is intended, throw exception
                    if (_override == false) throw new SheetAlreadyExistsException(sheet, this);

                    //If overwriting is intended, set bool to true, so that the sheet does not get created twice in the database
                    alreadyExists = true;
                    existingSheet = sheet;

                }

            }

            this.Progress = 25;


                if (alreadyExists == false)
                {

                    //Create database entry (only if sheet does not exists yet)
                    var _newsheet = manager.NewSheet(Piece.PieceID, Part.PartID);

                    this.Progress = 50;

                    //Extract page
                    PDFExtractor extractor = new PDFExtractor()
                    {
                        InputDocument = file,
                        OutputName = _newsheet.SheetID.ToString().PadLeft(8, '0'),
                        OutputPath = new DirectoryInfo(manager.ZebraConfig.TempDir)
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


                    ProcessFile(manager, _newsheet.SheetID.ToString().PadLeft(8, '0'), _newsheet);

                    this.Progress = 100;
                    IsImported = true;
                }

                else if (alreadyExists == true)
                {
                    this.Progress = 50;

                    //Extract page
                    PDFExtractor extractor = new PDFExtractor()
                    {
                        InputDocument = file,
                        OutputName = existingSheet.SheetID.ToString().PadLeft(8, '0'),
                        OutputPath = new DirectoryInfo(manager.ZebraConfig.TempDir)
                    };

                    this.Progress = 75;

                    try
                    {
                        //Extract the page from the batch
                        await extractor.ExtractPagesAsync(Pages.ToArray());
                    }
                    catch (Exception)
                    {
                        Progress = -1;
                        throw;
                    }

                    ProcessFile(manager, existingSheet.SheetID.ToString().PadLeft(8, '0'), existingSheet, FileImportMode.Copy, true);

                    this.Progress = 100;
                    IsImported = true;

                }
                  
        }

        private void ProcessFile(ZebraDBManager manager, string filename, Sheet _sheet, FileImportMode importMode = FileImportMode.Copy, bool _override = false)
        {
            //Push file to archive
            manager.Archive.PushFile(new FileInfo(manager.ZebraConfig.TempDir + $"/{filename}.pdf"), _sheet, importMode, _override);

            //Remove temp file
            var oldfile = new FileInfo(manager.ZebraConfig.TempDir + $"/{filename}.pdf");
            oldfile.Delete();
        }
    }
}
