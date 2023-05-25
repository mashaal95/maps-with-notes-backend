namespace MapNotesAPI.Interfaces
{
    public interface INotesRepository
    {
         Task<string> GetAllNotesFromLocation(string locationName);

         Task PostNotes(NotesTable notes);
    }
}