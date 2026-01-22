using WordRepeat.Core.Models;

namespace WordRepeat.Application.Abstractions
{
    public interface IHistoryTrainService
    {
        Task<Guid> AddAsync(HistoryTrain historyTrain, CancellationToken token);
        Task<List<HistoryTrain>> GetAllAsync(CancellationToken token);
        Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token);
        Task<int> UpdateCountAsync(int done, int total, DateOnly date, CancellationToken token);
        Task<HistoryTrain?> GetByIdAsync(Guid id, CancellationToken token);
    }
}