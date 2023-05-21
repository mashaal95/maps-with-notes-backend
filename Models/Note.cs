using System;
using System.Collections.Generic;

namespace MapNotesAPI;

public partial class Note
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Note1 { get; set; } = null!;

    public DateTime NoteDateTime { get; set; }
}
