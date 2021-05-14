using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zebra.Library;

namespace ZebraServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetlistsController : ControllerBase
    {
        private readonly SQLiteZebraContext _context;

        public SetlistsController(SQLiteZebraContext context)
        {
            _context = context;
        }

        // GET: api/Setlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetlistDTO>>> GetSetlist()
        {
            
            var SetlistList = await _context.Setlist.ToListAsync();
            List<SetlistDTO> SetlistDTOList = new List<SetlistDTO>();

            foreach (var sl in SetlistList)
            {
                var newSetlistDTO = new SetlistDTO { Name = sl.Name, Date = sl.Date, Location = sl.Location };
                newSetlistDTO.SetlistItems = new List<SetlistItemDTO>();

                foreach (var item in sl.SetlistItem)
                {
                    newSetlistDTO.SetlistItems.Add(new SetlistItemDTO { SetlistItemID = item.SetlistItemID, PieceName = item.Piece.Name, Position = item.Position});
                }

                SetlistDTOList.Add(newSetlistDTO);

            }
            return SetlistDTOList;
        }

        // GET: api/Setlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SetlistDTO>> GetSetlist(int id)
        {
            var sl = await _context.Setlist.FindAsync(id);

            if (sl == null)
            {
                return NotFound();
            }


            var newSetlistDTO = new SetlistDTO { Name = sl.Name, Date = sl.Date, Location = sl.Location };
            newSetlistDTO.SetlistItems = new List<SetlistItemDTO>();

            foreach (var item in sl.SetlistItem)
            {
                    newSetlistDTO.SetlistItems.Add(new SetlistItemDTO { SetlistItemID = item.SetlistItemID, PieceName = item.Piece.Name, Position = item.Position });
            }

            return newSetlistDTO;
        }

        // PUT: api/Setlists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSetlist(int id, Setlist setlist)
        {
            if (id != setlist.SetlistID)
            {
                return BadRequest();
            }

            _context.Entry(setlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SetlistExists(id))
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

        // POST: api/Setlists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Setlist>> PostSetlist(Setlist setlist)
        {
            _context.Setlist.Add(setlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSetlist", new { id = setlist.SetlistID }, setlist);
        }

        // DELETE: api/Setlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSetlist(int id)
        {
            var setlist = await _context.Setlist.FindAsync(id);
            if (setlist == null)
            {
                return NotFound();
            }

            _context.Setlist.Remove(setlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SetlistExists(int id)
        {
            return _context.Setlist.Any(e => e.SetlistID == id);
        }
    }
}
