using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Infrastructures;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Repositories
{
    public class HistoryAddRepository : IHistoryAddRepository
    {
        private readonly WordRepeatDbContext _context;
        public HistoryAddRepository(WordRepeatDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(HistoryAdd historyAdd, CancellationToken token)
        {
            HistoryAddEntity historyAddEntity = MapperEntity.ToHistoryAddEntity(historyAdd);
            await _context.HistoryAddTable.AddAsync(historyAddEntity, token);
            await _context.SaveChangesAsync(token);
            return historyAddEntity.Id;
        }

        public async Task<List<HistoryAdd>> GetAllAsync(CancellationToken token)
        {
            List<HistoryAddEntity> resultEntity = await _context.HistoryAddTable
                .AsNoTracking()
                .ToListAsync(token);
            List<HistoryAdd> result = new List<HistoryAdd>();
            foreach (HistoryAddEntity h in resultEntity)
            {
                result.Add(MapperEntity.FromHistoryAddEntity(h));
            }
            return result;
        }

        public async Task<bool> CheckByDateAsync(DateOnly date, CancellationToken token)
        {
            HistoryAddEntity? resultEntity = await _context.HistoryAddTable
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Date == date, token);
            if (resultEntity is null) return false;
            return true;
        }

        public async Task<int> UpdateCountAsync(int count, DateOnly date, CancellationToken token)
        {
            return await _context.HistoryAddTable
                .AsNoTracking()
                .Where(a => a.Date == date)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.CountAdd, a => a.CountAdd + count), token);
        }
    }
}
