using WordRepeat.Core.Models;

namespace WordRepeat.DataAccess.Sqlite.Abstractions
{
    public interface IHistoryTypesRepository
    {
        Task<Guid> AddAsync(HistoryTypes historyTypes, CancellationToken token);
        Task<List<HistoryTypes>> GetAllAsync(CancellationToken token);
    }
}