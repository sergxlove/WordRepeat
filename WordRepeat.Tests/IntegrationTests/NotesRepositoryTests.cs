using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite;
using WordRepeat.DataAccess.Sqlite.Repositories;

namespace WordRepeat.Tests.IntegrationTests
{
    public class NotesRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly WordRepeatDbContext _context;
        private readonly NotesRepository _repository;

        public NotesRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WordRepeatDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WordRepeatDbContext(options);
            _repository = new NotesRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNote_AndReturnId()
        {
            var createResult = Notes.Create("Test Title", "Test Content");
            var note = createResult.Value;
            var id = await _repository.AddAsync(note, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, id);
            var addedEntity = await _context.NotesTable.FindAsync(id);
            Assert.NotNull(addedEntity);
            Assert.Equal(note.Title, addedEntity.Title);
            Assert.Equal(note.Content, addedEntity.Content);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllNotes()
        {
            var note1 = Notes.Create("Title 1", "Content 1").Value;
            var note2 = Notes.Create("Title 2", "Content 2").Value;
            await _repository.AddAsync(note1, CancellationToken.None);
            await _repository.AddAsync(note2, CancellationToken.None);
            var result = await _repository.GetAllAsync(CancellationToken.None);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, n => n.Title == "Title 1" && n.Content == "Content 1");
            Assert.Contains(result, n => n.Title == "Title 2" && n.Content == "Content 2");
        }

        [Fact]
        public async Task DeleteAsync_WhenNoteExists_ShouldDeleteAndReturnOne()
        {
            var note = Notes.Create("Test Title", "Test Content").Value;
            var id = await _repository.AddAsync(note, CancellationToken.None);
            var deletedCount = await _repository.DeleteAsync(id, CancellationToken.None);
            Assert.Equal(1, deletedCount);
        }

        [Fact]
        public async Task DeleteAsync_WhenNoteNotExists_ShouldReturnZero()
        {
            var deletedCount = await _repository.DeleteAsync(Guid.NewGuid(), CancellationToken.None);
            Assert.Equal(0, deletedCount);
        }

        [Fact]
        public async Task UpdateTitleAsync_ShouldUpdateTitleAndDate()
        {
            var note = Notes.Create("Old Title", "Content").Value;
            var id = await _repository.AddAsync(note, CancellationToken.None);
            var newTitle = "New Title";
            var originalDateUpdate = note.DateUpdate;
            await Task.Delay(10);
            var updatedCount = await _repository.UpdateTitleAsync(id, newTitle, CancellationToken.None);
            _context.ChangeTracker.Clear();
            Assert.Equal(1, updatedCount);
            var updatedNote = await _context.NotesTable.FindAsync(id);
            Assert.NotNull(updatedNote);
            Assert.Equal(newTitle, updatedNote.Title);
            Assert.Equal("Content", updatedNote.Content);
            Assert.NotEqual(originalDateUpdate, updatedNote.DateUpdate);
            Assert.True(updatedNote.DateUpdate > originalDateUpdate);
        }

        [Fact]
        public async Task UpdateTitleAsync_WhenNoteNotExists_ShouldReturnZero()
        {
            var updatedCount = await _repository.UpdateTitleAsync(Guid.NewGuid(), "New Title", CancellationToken.None);
            Assert.Equal(0, updatedCount);
        }

        [Fact]
        public async Task UpdateContentAsync_ShouldUpdateContentAndDate()
        {
            var note = Notes.Create("Title", "Old Content").Value;
            var id = await _repository.AddAsync(note, CancellationToken.None);
            var newContent = "New Content";
            var originalDateUpdate = note.DateUpdate;
            await Task.Delay(10);
            var updatedCount = await _repository.UpdateContentAsync(id, newContent, CancellationToken.None);
            _context.ChangeTracker.Clear();
            Assert.Equal(1, updatedCount);
            var updatedNote = await _context.NotesTable.FindAsync(id);
            Assert.NotNull(updatedNote);
            Assert.Equal("Title", updatedNote.Title);
            Assert.Equal(newContent, updatedNote.Content);
            Assert.NotEqual(originalDateUpdate, updatedNote.DateUpdate);
            Assert.True(updatedNote.DateUpdate > originalDateUpdate);
        }

        [Fact]
        public async Task UpdateContentAsync_WhenNoteNotExists_ShouldReturnZero()
        {
            var updatedCount = await _repository.UpdateContentAsync(Guid.NewGuid(), "New Content", CancellationToken.None);
            Assert.Equal(0, updatedCount);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnNotesContainingTitle()
        {
            await _repository.AddAsync(Notes.Create("First Note", "Content 1").Value, CancellationToken.None);
            await _repository.AddAsync(Notes.Create("Second Document", "Content 2").Value, CancellationToken.None);
            await _repository.AddAsync(Notes.Create("Third Note", "Content 3").Value, CancellationToken.None);
            var searchResult = await _repository.SearchAsync("Note", CancellationToken.None);
            Assert.Equal(2, searchResult.Count);
            Assert.Contains(searchResult, n => n.Title == "First Note");
            Assert.Contains(searchResult, n => n.Title == "Third Note");
            Assert.DoesNotContain(searchResult, n => n.Title == "Second Document");
        }

        [Fact]
        public async Task SearchAsync_WithEmptySearch_ShouldReturnAllNotes()
        {
            await _repository.AddAsync(Notes.Create("Note 1", "Content 1").Value, CancellationToken.None);
            await _repository.AddAsync(Notes.Create("Note 2", "Content 2").Value, CancellationToken.None);
            var searchResult = await _repository.SearchAsync("", CancellationToken.None);
            Assert.Equal(2, searchResult.Count);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyList()
        {
            await _repository.AddAsync(Notes.Create("Note 1", "Content 1").Value, CancellationToken.None);
            var searchResult = await _repository.SearchAsync("NonExistent", CancellationToken.None);
            Assert.Empty(searchResult);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
