using WordRepeat.Core.Models;

namespace WordRepeat.DataAccess.Sqlite.Abstractions
{
    public interface IHistoryTrainRepository
    {
        Task<Guid> AddAsync(HistoryTrain historyTrain, CancellationToken token);
        Task<List<HistoryTrain>> GetAllAsync(CancellationToken token);
        Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token);
        Task<int> UpdateCountAsync(int done, int total, DateOnly date, CancellationToken token);
    }
}