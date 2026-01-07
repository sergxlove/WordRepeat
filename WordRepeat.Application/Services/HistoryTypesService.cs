using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class HistoryTypesService : IHistoryTypesService
    {
        private readonly IHistoryTypesRepository _repository;
        public HistoryTypesService(IHistoryTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> AddAsync(HistoryTypes historyTypes, CancellationToken token)
        {
            return await _repository.AddAsync(historyTypes, token);
        }
        public async Task<List<HistoryTypes>> GetAllAsync(CancellationToken token)
        {
            return await _repository.GetAllAsync(token);
        }
    }
}
