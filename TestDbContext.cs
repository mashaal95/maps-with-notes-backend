using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MapNotesAPI;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NotesTable> NotesTables { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:TestDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotesTable>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__NotesTab__0BBF6EE6EC2D2EE6");

            entity.ToTable("NotesTable");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.LocationName).HasColumnName("location_name");
            entity.Property(e => e.NotesText).HasColumnName("notes_text");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserTable");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
