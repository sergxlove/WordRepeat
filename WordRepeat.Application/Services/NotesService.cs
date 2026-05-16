using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _repository;
        public NotesService(INotesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> AddAsync(Notes note, CancellationToken token)
        {
            return await _repository.AddAsync(note, token);
        }
        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<List<Notes>> GetAllAsync(CancellationToken token)
        {
            return await _repository.GetAllAsync(token);
        }
        public async Task<List<Notes>> SearchAsync(string title, CancellationToken token)
        {
            return await _repository.SearchAsync(title, token);
        }
        public async Task<int> UpdateContentAsync(Guid id, string content, CancellationToken token)
        {
            return await _repository.UpdateContentAsync(id, content, token);
        }
        public async Task<int> UpdateTitleAsync(Guid id, string title, CancellationToken token)
        {
            return await _repository.UpdateTitleAsync(id, title, token);
        }
    }
}
