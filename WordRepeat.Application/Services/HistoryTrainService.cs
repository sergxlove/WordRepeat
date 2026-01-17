using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class HistoryTrainService : IHistoryTrainService
    {
        private readonly IHistoryTrainRepository _repository;
        public HistoryTrainService(IHistoryTrainRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> AddAsync(HistoryTrain historyTrain, CancellationToken token)
        {
            return await _repository.AddAsync(historyTrain, token);
        }
        public async Task<List<HistoryTrain>> GetAllAsync(CancellationToken token)
        {
            return await _repository.GetAllAsync(token);
        }
        public async Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token)
        {
            return await _repository.CheckByDateAsync(date, token);
        }
        public async Task<int> UpdateCountAsync(int done, int total, DateOnly date, 
            CancellationToken token)
        {
            return await _repository.UpdateCountAsync(done, total, date, token);
        }
    }
}
