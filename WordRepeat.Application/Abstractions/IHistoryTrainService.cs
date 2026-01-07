using WordRepeat.Core.Models;

namespace WordRepeat.Application.Abstractions
{
    public interface IHistoryTrainService
    {
        Task<Guid> AddAsync(HistoryTrain historyTrain, CancellationToken token);
        Task<List<HistoryTrain>> GetAllAsync(CancellationToken token);
    }
}