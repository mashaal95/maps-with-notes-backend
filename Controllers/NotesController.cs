using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapNotesAPI;

namespace MapNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly TestDbContext _context;

        public NotesController(TestDbContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotesTable>>> GetNotesTables()
        {
          if (_context.NotesTables == null)
          {
              return NotFound();
          }
            return await _context.NotesTables.ToListAsync();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotesTable>> GetNotesTable(int id)
        {
          if (_context.NotesTables == null)
          {
              return NotFound();
          }
            var notesTable = await _context.NotesTables.FindAsync(id);

            if (notesTable == null)
            {
                return NotFound();
            }

            return notesTable;
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotesTable(int id, NotesTable notesTable)
        {
            if (id != notesTable.MessageId)
            {
                return BadRequest();
            }

            _context.Entry(notesTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesTableExists(id))
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

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NotesTable>> PostNotesTable(NotesTable notesTable)
        {
          if (_context.NotesTables == null)
          {
              return Problem("Entity set 'TestDbContext.NotesTables'  is null.");
          }
            _context.NotesTables.Add(notesTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotesTable", new { id = notesTable.MessageId }, notesTable);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotesTable(int id)
        {
            if (_context.NotesTables == null)
            {
                return NotFound();
            }
            var notesTable = await _context.NotesTables.FindAsync(id);
            if (notesTable == null)
            {
                return NotFound();
            }

            _context.NotesTables.Remove(notesTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotesTableExists(int id)
        {
            return (_context.NotesTables?.Any(e => e.MessageId == id)).GetValueOrDefault();
        }
    }
}
