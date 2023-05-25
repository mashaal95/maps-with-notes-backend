using MapNotesAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MapNotesAPI.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly TestDbContext _context;

        public UserRepository(TestDbContext context)
        {
            _context = context;
        }

        public Task<Guid> GetUserIdFromUsername(string username)
        {
            var userId = from userTable in _context.UserTables
            where userTable.Username == username
            select userTable.UserId;

            return userId.FirstAsync(); 
        }

        public UserTable Login(string username, string password)
        {
            var user = new UserTable();

            try {
           user = (from loginUser in _context.UserTables
           where loginUser.Username == username && loginUser.Password == password
           select loginUser).First();
            }
            catch(InvalidOperationException exception)
            {
                return new UserTable();
            }

            return user;

        
        }

        public async Task PostNotes(NotesTable notes)
        {
        
        _context.NotesTables.Add(notes);
        
        await _context.SaveChangesAsync();


        }


    }
}