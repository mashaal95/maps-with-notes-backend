using System;
using System.Collections.Generic;

namespace MapNotesAPI;

public partial class UserTable
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
