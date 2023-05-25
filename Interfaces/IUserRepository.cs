using Microsoft.AspNetCore.Mvc;

namespace MapNotesAPI.Interfaces
{
    public interface IUserRepository
    {
         Task<Guid> GetUserIdFromUsername(string username);

         UserTable Login(string username, string password);

    }
}