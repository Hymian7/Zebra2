using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Zebra.Library;
using Zebra.Library.PdfHandling;

namespace Zebra.PdfHandling
{
    public class ImportCandidateImporter
    {
        private ZebraDBManager Manager { get; set; }
        public PDFtkSharp.PDFExtractor Extractor { get; set; }

        public ImportCandidateImporter(ZebraDBManager zebraDBManager)
        {
            Manager = zebraDBManager;
            Extractor = new PDFtkSharp.PDFExtractor();
        }


        /// <summary>
        /// Imports the given ImportCandidate and adds a new entry in the database. The pages to be imported are extracted from the original document
        /// into a temporary PDF file, which is then moved to the archive folder.
        /// </summary>
        /// <param name="importCandidate">The ImportCandidate that shall be imported into the database.</param>
        /// <returns></returns>
        /// <exception cref="SheetAlreadyExistsException"></exception>
        public async Task ImportImportCandidate(ImportCandidate importCandidate)
        {

            //Check if Sheet is already in Database
            //Throw if true

            foreach (var sheet in importCandidate.AssignedPiece.Sheet)
            {
                if (sheet.Part == importCandidate.AssignedPart)
                {
                    throw new SheetAlreadyExistsException(sheet, importCandidate);
                }
            }

            //Create database entry (only if sheet does not exists yet)

            Sheet newSheet = new Sheet(importCandidate.AssignedPart, importCandidate.AssignedPiece);
            await Manager.Context.Sheet.AddAsync(newSheet);
            await Manager.Context.SaveChangesAsync();

            //Extract page
            Extractor.InputFile = new FileInfo(importCandidate.DocumentPath);
            Extractor.OutputPath = new DirectoryInfo(Manager.ZebraConfig.TempDirectory);
            Extractor.OutputName = FileNameResolver.GetFileName(newSheet);

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
                Manager.Archive.PushFile(new FileInfo(Manager.ZebraConfig.TempDirectory + "\\" + FileNameResolver.GetFileName(newSheet)), newSheet, FileImportMode.Copy, false);
            }
            catch (Exception)
            {
                //Undo Sheet creation in Database
                Manager.Context.Sheet.Remove(newSheet);
                Manager.Context.SaveChanges();

                throw;
            }
            finally
            {
                //Remove temp file if it has been created
                if (File.Exists(Manager.ZebraConfig.TempDirectory + "\\" + FileNameResolver.GetFileName(newSheet)))
                {
                    File.Delete(Manager.ZebraConfig.TempDirectory + "\\" + FileNameResolver.GetFileName(newSheet));
                }
            }
            

            


        }
    }
}
