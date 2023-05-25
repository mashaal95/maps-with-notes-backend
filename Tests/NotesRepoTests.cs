using MapNotesAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace MapNotesAPI.Repositories.Tests
{
    public class NotesRepositoryTests
    {
        [Fact]
        public async Task GetAllNotesFromLocation_Should_Return_JsonString()
        {
            // Arrange
            var locationName = "TestLocation";
            var notes = new List<NotesTable>
            {
                new NotesTable { MessageId = 1, UserId = 1, LocationName = locationName, NotesText = "Note 1" },
                new NotesTable { MessageId = 2, UserId = 2, LocationName = locationName, NotesText = "Note 2" }
            };

            var users = new List<UserTable>
            {
                new UserTable { UserId = 1, Username = "User1" },
                new UserTable { UserId = 2, Username = "User2" }
            };

            var notesQueryable = notes.AsQueryable();
            var usersQueryable = users.AsQueryable();

            var notesMockSet = new Mock<DbSet<NotesTable>>();
            notesMockSet.As<IQueryable<NotesTable>>().Setup(m => m.Provider).Returns(notesQueryable.Provider);
            notesMockSet.As<IQueryable<NotesTable>>().Setup(m => m.Expression).Returns(notesQueryable.Expression);
            notesMockSet.As<IQueryable<NotesTable>>().Setup(m => m.ElementType).Returns(notesQueryable.ElementType);
            notesMockSet.As<IQueryable<NotesTable>>().Setup(m => m.GetEnumerator()).Returns(notesQueryable.GetEnumerator());

            var usersMockSet = new Mock<DbSet<UserTable>>();
            usersMockSet.As<IQueryable<UserTable>>().Setup(m => m.Provider).Returns(usersQueryable.Provider);
            usersMockSet.As<IQueryable<UserTable>>().Setup(m => m.Expression).Returns(usersQueryable.Expression);
            usersMockSet.As<IQueryable<UserTable>>().Setup(m => m.ElementType).Returns(usersQueryable.ElementType);
            usersMockSet.As<IQueryable<UserTable>>().Setup(m => m.GetEnumerator()).Returns(usersQueryable.GetEnumerator());

            var dbContextMock = new Mock<TestDbContext>();
            dbContextMock.Setup(c => c.NotesTables).Returns(notesMockSet.Object);
            dbContextMock.Setup(c => c.UserTables).Returns(usersMockSet.Object);

            var repository = new NotesRepository(dbContextMock.Object);

            // Act
            var result = await repository.GetAllNotesFromLocation(locationName);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var deserializedNotes = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(result);
            Assert.Equal(2, deserializedNotes.Count());
        }

        [Fact]
        public async Task PostNotes_Should_Save_Notes_To_Context()
        {
            // Arrange
            var notes = new NotesTable { MessageId = 1, UserId = 1, LocationName = "TestLocation", NotesText = "Test note" };

            var dbContextMock = new Mock<TestDbContext>();
            var notesMockSet = new Mock<DbSet<NotesTable>>();

            dbContextMock.Setup(c => c.NotesTables).Returns(notesMockSet.Object);

            var repository = new NotesRepository(dbContextMock.Object);

            // Act
            await repository.PostNotes(notes);

            // Assert
            notesMockSet.Verify(set => set.Add(notes), Times.Once);
            dbContextMock.Verify(context => context.SaveChangesAsync(), Times.Once);
        }
    }
}
