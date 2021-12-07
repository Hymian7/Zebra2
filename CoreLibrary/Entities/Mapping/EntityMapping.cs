using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library.Mapping
{
    public static class EntityMapping
    {
        public static PieceDTO ToDTO(this Piece piece)
        {
            var newPieceDTO = new PieceDTO()
            {
                Name = piece.Name,
                Arranger = piece.Arranger,
                PieceID = piece.PieceID
            };

            newPieceDTO.Sheet = new List<SheetDTO>();
            newPieceDTO.Setlist = new List<SetlistDTO>();

            return newPieceDTO;
        }

        public static SheetDTO ToDTO(this Sheet sheet)
        {
            var newSheetDTO = new SheetDTO()
            {
                Name = sheet.Piece.Name + " " + sheet.Part.Name,
                PartName = sheet.Part.Name,
                PieceName = sheet.Piece.Name,
                SheetID = sheet.SheetID,
                PartID = sheet.Part.PartID,
                PieceID = sheet.Piece.PieceID
            };


            return newSheetDTO;
        }

        public static PartDTO ToDTO(this Part part)
        {
            var newPartDTO = new PartDTO()
            {
                Name = part.Name,
                PartID = part.PartID,
                Position = part.Position
            };

            newPartDTO.Sheet = new List<SheetDTO>();

            return newPartDTO;
        }

        public static SetlistDTO ToDTO(this Setlist setlist)
        {
            var newSetlistDTO = new SetlistDTO()
            {
                SetlistID = setlist.SetlistID,
                Name = setlist.Name,
                Date = setlist.Date,
                Location = setlist.Location
            };

            newSetlistDTO.SetlistItems = new List<SetlistItemDTO>();

            return newSetlistDTO;
        }

        public static SetlistItemDTO ToDTO(this SetlistItem sli)
        {
            var newSLI = new SetlistItemDTO
            {
                
                PieceID = sli.Piece.PieceID,
                PieceName = sli.Piece.Name,
                Position = sli.Position,
                SetlistItemID = sli.SetlistItemID
            };

            return newSLI;
        }
    }
}
