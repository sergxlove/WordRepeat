using WordRepeat.Core.Models;

namespace WordRepeat.Application.Abstractions
{
    public interface IHistoryAddServices
    {
        Task<Guid> AddAsync(HistoryAdd historyAdd, CancellationToken token);
        Task<List<HistoryAdd>> GetAllAsync(CancellationToken token);
        Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token);
        Task<int> UpdateCountAsync(int count, DateOnly date, CancellationToken token);
    }
}