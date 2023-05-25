using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapNotesAPI;
using MapNotesAPI.Interfaces;

namespace MapNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TestDbContext _context;

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // // GET: api/User
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        // {
        //   if (_context.UserTables == null)
        //   {
        //       return NotFound();
        //   }
        //     return await _context.UserTables.ToListAsync();
        // }

        // GET: api/User/5
        [HttpGet("{username}")]
        public async Task<Guid> GetUserIdFromUsername(String username)
        {
        //   if (_context.UserTables == null)
        //   {
        //       return NotFound();
        //   }
            // var userTable = await _context.UserTables.FindAsync(id);

            // if (userTable == null)
            // {
            //     return NotFound();
            // }
            return await _userRepository.GetUserIdFromUsername(username);

            // return userTable;
        }

        [HttpPost("login")]
        public IActionResult Login(String username, String password) {
            
            
            var user = _userRepository.Login(username, password);

            if ( user.Username != null)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }

        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(Guid id, UserTable userTable)
        {
            if (id != userTable.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUserTable(UserTable userTable)
        {
          if (_context.UserTables == null)
          {
              return Problem("Entity set 'TestDbContext.UserTables'  is null.");
          }
            _context.UserTables.Add(userTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTableExists(userTable.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserTable", new { id = userTable.UserId }, userTable);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(Guid id)
        {
            if (_context.UserTables == null)
            {
                return NotFound();
            }
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(Guid id)
        {
            return (_context.UserTables?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
