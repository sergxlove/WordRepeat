using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Infrastructures;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Repositories
{
    public class HistoryTrainRepository : IHistoryTrainRepository
    {
        private readonly WordRepeatDbContext _context;
        public HistoryTrainRepository(WordRepeatDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(HistoryTrain historyTrain, CancellationToken token)
        {
            HistoryTrainEntity historyTrainEntity = MapperEntity.ToHistoryTrainEntity(historyTrain);
            await _context.HistoryTrainTable.AddAsync(historyTrainEntity, token);
            await _context.SaveChangesAsync(token);
            return historyTrainEntity.Id;
        }

        public async Task<List<HistoryTrain>> GetAllAsync(CancellationToken token)
        {
            List<HistoryTrainEntity> resultEntity = await _context.HistoryTrainTable
                .AsNoTracking()
                .ToListAsync(token);
            List<HistoryTrain> result = new List<HistoryTrain>();
            foreach (HistoryTrainEntity h in resultEntity)
            {
                result.Add(MapperEntity.FromHistoryTrainEntity(h));
            }
            return result;
        }
    }
}
