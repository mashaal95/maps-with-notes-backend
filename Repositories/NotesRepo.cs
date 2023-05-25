using MapNotesAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MapNotesAPI.Repositories
{
    public class NotesRepository : INotesRepository
    {

        private readonly TestDbContext _context;

        public NotesRepository(TestDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetAllNotesFromLocation(string locationName)
        {
            var notes = await ( 
                from note in _context.NotesTables
                where note.LocationName == locationName
                join user in _context.UserTables on note.UserId equals user.UserId
                select new 
                {
                    messageId = note.MessageId ,
                    userId = user.UserId,
                    username = user.Username,
                    locationName = note.LocationName,
                    notesText = note.NotesText
                }
            ).ToListAsync();

            string json = JsonConvert.SerializeObject(notes);

            return json;
        }

        public async Task PostNotes(NotesTable notes)
        {
        
        _context.NotesTables.Add(notes);
        
        await _context.SaveChangesAsync();


        }


    }
}