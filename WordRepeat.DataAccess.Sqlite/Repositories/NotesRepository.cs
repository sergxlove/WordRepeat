using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Infrastructures;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Infrastructures;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly WordRepeatDbContext _context;
        public NotesRepository(WordRepeatDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Notes note, CancellationToken token)
        {
            NotesEntity notesEntity = MapperEntity.ToNotesEntity(note);
            await _context.NotesTable.AddAsync(notesEntity, token);
            await _context.SaveChangesAsync(token);
            return note.Id;
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.NotesTable
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }

        public async Task<int> UpdateTitleAsync(Guid id, string title, CancellationToken token)
        {
            return await _context.NotesTable
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Title, title)
                .SetProperty(a => a.DateUpdate, DateTime.UtcNow), token);
        }

        public async Task<int> UpdateContentAsync(Guid id, string content, CancellationToken token)
        {
            return await _context.NotesTable
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Content, content)
                .SetProperty(a => a.DateUpdate, DateTime.UtcNow), token);
        }

        public async Task<List<Notes>> GetAllAsync(CancellationToken token)
        {
            List<NotesEntity> notesEntity = await _context.NotesTable
                .ToListAsync(token);
            List<Notes> result = new List<Notes>();
            foreach (NotesEntity n in notesEntity)
            {
                ResultCreateModel<Notes> newNote = Notes.Create(n.Id, n.Title, n.Content, n.DateUpdate);
                if (!string.IsNullOrEmpty(newNote.Error)) continue;
                result.Add(newNote.Value);
            }
            return result;
        }

        public async Task<List<Notes>> SearchAsync(string title, CancellationToken token)
        {
            List<NotesEntity> notesEntity = await _context.NotesTable
                .Where(a => a.Title.Contains(title))
                .ToListAsync(token);
            List<Notes> result = new List<Notes>();
            foreach (NotesEntity n in notesEntity)
            {
                ResultCreateModel<Notes> newNote = Notes.Create(n.Id, n.Title, n.Content, n.DateUpdate);
                if (!string.IsNullOrEmpty(newNote.Error)) continue;
                result.Add(newNote.Value);
            }
            return result;
        }
    }
}
