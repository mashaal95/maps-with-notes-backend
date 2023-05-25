namespace MapNotesAPI;

public partial class NotesTable
{
    public int MessageId { get; set; }

    public Guid UserId { get; set; }

    public string LocationName { get; set; } = null!;

    public string NotesText { get; set; } = null!;
}


