using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebra.Library;
using Zebra.Library.Services;

namespace Zebra.Library.PdfHandling
{
    public class ImportCandidateImporter : IImportCandidateImporter
    {
        private IZebraDBManager Manager { get; set; }
        private PDFtkSharp.PDFExtractor Extractor { get; set; }

        private FilePathService _fileNameService;

        private ZebraContext _context;

        private ArchiveService _archiveService;

        public ImportCandidateImporter(FilePathService fileNameService, ZebraContext context, ArchiveService archiveService)
        {
            _fileNameService = fileNameService;
            _archiveService = archiveService;
            _context = context;
            Extractor = new PDFtkSharp.PDFExtractor();
        }

        ~ImportCandidateImporter()
        {
            foreach (var file in Directory.GetFiles(_fileNameService.GetFolderPath(FolderType.Temp)))
            {
                try
                { System.IO.File.Delete(file); }
                catch(Exception)
                { }
            }
        }


        /// <summary>
        /// Imports the given ImportCandidate and adds a new entry in the database. The pages to be imported are extracted from the original document
        /// into a temporary PDF file, which is then moved to the archive folder.
        /// </summary>
        /// <param name="importCandidate">The ImportCandidate that shall be imported into the database.</param>
        /// <returns></returns>
        /// <exception cref="SheetAlreadyExistsException"></exception>
        public async Task ImportImportCandidateAsync(ImportCandidate importCandidate)
        {
            int newId = -1;
            bool isAdded = false;

            try
            {
                newId = await AddSheetToDatabase(importCandidate);
                await StoreFileInArchive(importCandidate, newId);
                isAdded = true;
            }
            catch(Exception)
            {
                // Remove sheet from database if it was added
                if (isAdded == false)
                {
                    var newSheet = await _context.Sheet.FindAsync(newId);
                    if (newSheet != null)
                    {
                        _context.Sheet.Remove(newSheet);
                        await _context.SaveChangesAsync();
                    }
                }

                throw;
            }        
        }

        private async Task StoreFileInArchive(ImportCandidate importCandidate, int newId)
        {
            string newFilePath = _fileNameService.GetFilePath(FolderType.Archive, newId);
            FileInfo newFile = new FileInfo(newFilePath);

            string extractedFilePath = _fileNameService.GetFilePath(FolderType.Temp, newId);
            FileInfo extractedFile = new FileInfo(extractedFilePath);

            //Extract page
            Extractor.InputFile = new FileInfo(_fileNameService.GetFilePath(FolderType.Temp,importCandidate.DocumentId));
            Extractor.OutputPath = new DirectoryInfo(extractedFile.DirectoryName);
            Extractor.OutputName = extractedFile.Name;

            int[] pages = new int[importCandidate.Pages.Count];

            for (int i = 0; i < importCandidate.Pages.Count; i++)
            {
                pages[i] = importCandidate.Pages[i].PageNumber;
            }

            try
            {
                //Extract pages to be imported and save into temp PDF file
                await Extractor.ExtractAsync(pages);

                //Push file to archive
                _archiveService.PushFile(extractedFile, newFile, FileImportMode.Copy, false);
            }
            finally
            {
                //Remove temp file if it has been created
                if (extractedFile.Exists)
                {
                    extractedFile.Delete();
                }
            }
        }

        private async Task<int> AddSheetToDatabase(ImportCandidate importCandidate)
        {
            // Check if ImportCandidate is fully assigned
            if (importCandidate.IsAssigned == false) throw new Exception("Import Candidate is not fully assigned");

            // Check if ImportCandidate Guid is known by the session
            if (!File.Exists(_fileNameService.GetFilePath(FolderType.Temp, importCandidate.DocumentId))) throw new Exception("Import Candidate is not known by current session");

            // Check if sheet already exists
            var existingSheet = _context.Sheet.SingleOrDefault<Sheet>(s => s.Part.PartID == importCandidate.AssignedPart.PartID && s.Piece.PieceID == importCandidate.AssignedPiece.PieceID);
            if (existingSheet != null) throw new Exception("Sheet already exists!");

            var newSheet = new Sheet(await _context.Part.FindAsync(importCandidate.AssignedPart.PartID), await _context.Piece.FindAsync(importCandidate.AssignedPiece.PieceID));

            var newEntity = await _context.Sheet.AddAsync(newSheet);
            await _context.SaveChangesAsync();

            return newEntity.Entity.SheetID;
        }
    }
}
