using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class HistoryTrainService
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
    }
}
