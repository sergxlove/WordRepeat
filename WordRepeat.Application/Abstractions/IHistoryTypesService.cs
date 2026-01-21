using WordRepeat.Core.Models;

namespace WordRepeat.Application.Abstractions
{
    public interface IHistoryTypesService
    {
        Task<Guid> AddAsync(HistoryTypes historyTypes, CancellationToken token);
        Task<List<HistoryTypes>> GetAllAsync(CancellationToken token);
        Task<int> CountAsync(CancellationToken token);
        Task<List<HistoryTypes>> GetByPaginationAsync(int currentPage, int sizePage, CancellationToken token);
    }
}