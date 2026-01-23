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
        public async Task<HistoryTrain?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _repository.GetByIdAsync(id, token);
        }
        public async Task<int> GetTrainedTodayAsync(CancellationToken token)
        {
            return await _repository.GetTrainedTodayAsync(token);
        }
        public async Task<int> GetAccuracyByWeekAsync(CancellationToken token)
        {
            return await _repository.GetAccuracyByWeekAsync(token);
        }
        public async Task<int> GetAccuracyByAllAsync(CancellationToken token)
        {
            return await _repository.GetAccuracyByAllAsync(token);
        }
        public async Task<int> GetAccuracyByMonthAsync(CancellationToken token)
        {
            return await _repository.GetAccuracyByMonthAsync(token);
        }
        public async Task<int> CountAsync(CancellationToken token)
        {
            return await _repository.CountAsync(token);
        }
        public async Task<int> GetCountWrongAsync(CancellationToken token)
        {
            return await _repository.GetCountWrongAsync(token);
        }
        public async Task<int> GetCountDoneAsync(CancellationToken token)
        {
            return await _repository.GetCountDoneAsync(token);
        }
        public async Task<int> GetStreakAsync(CancellationToken token)
        {
            return await _repository.GetStreakAsync(token);
        }
    }
}
