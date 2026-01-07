using WordRepeat.Core.Models;

namespace WordRepeat.DataAccess.Sqlite.Abstractions
{
    public interface IHistoryAddRepository
    {
        Task<Guid> AddAsync(HistoryAdd historyAdd, CancellationToken token);
        Task<List<HistoryAdd>> GetAllAsync(CancellationToken token);
    }
}