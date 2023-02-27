using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zebra.Library;
using Zebra.Library.Mapping;

namespace ZebraServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiecesController : ControllerBase
    {
        private readonly ZebraContext _context;

        public PiecesController(ZebraContext context)
        {
            _context = context;
        }

        // GET: api/Pieces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PieceDTO>>> GetPiece()
        {
            var allpieces = await _context.Piece.ToListAsync();
            var allpiecesDTO = new List<PieceDTO>();

            foreach (var piece in allpieces)
            {
                var pieceDTO = piece.ToDTO();
                pieceDTO.Sheet = new List<SheetDTO>();
                pieceDTO.Setlist = new List<SetlistDTO>();

                //foreach (var sheet in piece.Sheet)
                //{
                //    pieceDTO.Sheet.Add(sheet.ToDTO());
                //}
                //
                //foreach (var setlistitem in piece.SetlistItem)
                //{
                //    if(!pieceDTO.Setlist.Contains(setlistitem.Setlist.ToDTO()))
                //           pieceDTO.Setlist.Add(setlistitem.Setlist.ToDTO());
                //}

                allpiecesDTO.Add(pieceDTO);
            }

            return allpiecesDTO;

            
        }

        // GET: api/Pieces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PieceDTO>> GetPiece(int id)
        {
            var piece = await _context.Piece.FindAsync(id);

            if (piece == null)
            {
                return NotFound();
            }

            var pieceDTO = piece.ToDTO();

            foreach (var sheet in piece.Sheet)
            {
                pieceDTO.Sheet.Add(sheet.ToDTO());
            }
            
            foreach (var setlistitem in piece.SetlistItem)
            {
                if(!pieceDTO.Setlist.Contains(setlistitem.Setlist.ToDTO()))
                       pieceDTO.Setlist.Add(setlistitem.Setlist.ToDTO());
            }

            return pieceDTO;
        }

        // PUT: api/Pieces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPiece(int id, Piece piece)
        {
            if (id != piece.PieceID)
            {
                return BadRequest();
            }

            _context.Entry(piece).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PieceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pieces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Piece>> PostPiece(Piece piece)
        {
            _context.Piece.Add(piece);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPiece", new { id = piece.PieceID }, piece);
        }

        // DELETE: api/Pieces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePiece(int id)
        {
            var piece = await _context.Piece.FindAsync(id);
            if (piece == null)
            {
                return NotFound();
            }

            _context.Piece.Remove(piece);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PieceExists(int id)
        {
            return _context.Piece.Any(e => e.PieceID == id);
        }
    }
}
