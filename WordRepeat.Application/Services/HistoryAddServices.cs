using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class HistoryAddServices
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
    }
}
