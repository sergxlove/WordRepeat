using WordRepeat.Core.Models;

namespace WordRepeat.DataAccess.Sqlite.Abstractions
{
    public interface INotesRepository
    {
        Task<Guid> AddAsync(Notes note, CancellationToken token);
        Task<int> DeleteAsync(Guid id, CancellationToken token);
        Task<List<Notes>> GetAllAsync(CancellationToken token);
        Task<List<Notes>> SearchAsync(string title, CancellationToken token);
        Task<int> UpdateContentAsync(Guid id, string content, CancellationToken token);
        Task<int> UpdateTitleAsync(Guid id, string title, CancellationToken token);
    }
}