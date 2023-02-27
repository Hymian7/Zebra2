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
    public class PartsController : ControllerBase
    {
        private readonly ZebraContext _context;

        public PartsController(ZebraContext context)
        {
            _context = context;
        }

        // GET: api/Parts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartDTO>>> GetPart()
        {
            var allparts = await _context.Part.ToListAsync();
            var allpartsDTO = new List<PartDTO>();

            foreach (var part in allparts)
            {
                allpartsDTO.Add(part.ToDTO());
            }

            return allpartsDTO;
        }

        // GET: api/Parts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PartDTO>> GetPart(int id)
        {
            var part = await _context.Part.FindAsync(id);

            if (part == null)
            {
                return NotFound();
            }

            return part.ToDTO() ;
        }

        // PUT: api/Parts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPart(int id, Part part)
        {
            if (id != part.PartID)
            {
                return BadRequest();
            }

            _context.Entry(part).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(id))
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

        // POST: api/Parts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Part>> PostPart(PartDTO part)
        {
            var newPart = new Part(part.Name) { Position = part.Position };

            _context.Part.Add(newPart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPart", new { id = part.PartID }, part);
        }

        // DELETE: api/Parts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(int id)
        {
            var part = await _context.Part.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            _context.Part.Remove(part);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartExists(int id)
        {
            return _context.Part.Any(e => e.PartID == id);
        }
    }
}
