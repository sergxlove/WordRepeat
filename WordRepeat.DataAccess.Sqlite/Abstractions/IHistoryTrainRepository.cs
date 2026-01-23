using WordRepeat.Core.Models;

namespace WordRepeat.DataAccess.Sqlite.Abstractions
{
    public interface IHistoryTrainRepository
    {
        Task<Guid> AddAsync(HistoryTrain historyTrain, CancellationToken token);
        Task<List<HistoryTrain>> GetAllAsync(CancellationToken token);
        Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token);
        Task<int> UpdateCountAsync(int done, int total, DateOnly date, CancellationToken token);
        Task<HistoryTrain?> GetByIdAsync(Guid id, CancellationToken token);
        Task<int> GetTrainedTodayAsync(CancellationToken token);
        Task<int> GetAccuracyByWeekAsync(CancellationToken token);
        Task<int> GetAccuracyByAllAsync(CancellationToken token);
        Task<int> GetAccuracyByMonthAsync(CancellationToken token);
        Task<int> CountAsync(CancellationToken token);
        Task<int> GetCountWrongAsync(CancellationToken token);
        Task<int> GetCountDoneAsync(CancellationToken token);
        Task<int> GetStreakAsync(CancellationToken token);
    }
}