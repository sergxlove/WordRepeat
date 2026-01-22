using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class HistoryAddServices : IHistoryAddServices
    {
        private readonly IHistoryAddRepository _repository;
        public HistoryAddServices(IHistoryAddRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> AddAsync(HistoryAdd historyAdd, CancellationToken token)
        {
            return await _repository.AddAsync(historyAdd, token);
        }
        public async Task<List<HistoryAdd>> GetAllAsync(CancellationToken token)
        {
            return await _repository.GetAllAsync(token);
        }
        public async Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token)
        {
            return await _repository.CheckByDateAsync(date, token);
        }
        public async Task<int> UpdateCountAsync(int count, DateOnly date, CancellationToken token)
        {
            return await _repository.UpdateCountAsync(count, date, token);
        }
        public async Task<HistoryAdd?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetByIdAsync(id, token);
        }
    }
}
