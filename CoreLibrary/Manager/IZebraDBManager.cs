using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zebra.Library.PdfHandling;

namespace Zebra.Library
{
    public interface IZebraDBManager
    {
        ZebraConfig ZebraConfig { get; }

        // Pieces
        Task<List<PieceDTO>> GetAllPiecesAsync();
        Task<PieceDTO> GetPieceAsync(int id);
        
        // Parts
        Task<List<PartDTO>> GetAllPartsAsync();
        Task<PartDTO> GetPartAsync(int id);
        Task<PartDTO> PostPartAsync(PartDTO newPart);
        
        // Setlists
        Task<List<SetlistDTO>> GetAllSetlistsAsync();
        Task<SetlistDTO> GetSetlistAsync(int id);
        
        // Sheets
        Task<SheetDTO> GetSheetAsync(int id);

        Task<string> GetPDFPathAsync(int id);

        Task<ImportCandidate> GetImportCandidateAsync(string filepath);
        Task ImportImportCandidateAsync(ImportCandidate ic);
    }
}