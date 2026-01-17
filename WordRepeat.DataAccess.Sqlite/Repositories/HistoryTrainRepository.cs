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

        public async Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token)
        {
            HistoryTrainEntity? result = await _context.HistoryTrainTable
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Date == date, token);
            if (result is null) return false;
            return true;
        }

        public async Task<int> UpdateCountAsync(int done, int total, DateOnly date, 
            CancellationToken token)
        {
            return await _context.HistoryTrainTable
                .AsNoTracking()
                .Where(a => a.Date == date)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Result, a => a.Result + done)
                .SetProperty(a => a.Total, a => a.Total + total), token);
        }
    }
}
