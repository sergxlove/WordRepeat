using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Infrastructures;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Repositories
{
    public class HistoryTypesRepository : IHistoryTypesRepository
    {
        private readonly WordRepeatDbContext _context;
        public HistoryTypesRepository(WordRepeatDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(HistoryTypes historyTypes, CancellationToken token)
        {
            HistoryTypesEntity historyTypesEntity = MapperEntity.ToHistoryTypesEntity(historyTypes);
            await _context.HistoryTypesTable.AddAsync(historyTypesEntity, token);
            await _context.SaveChangesAsync(token);
            return historyTypesEntity.Id;
        }

        public async Task<List<HistoryTypes>> GetAllAsync(CancellationToken token)
        {
            List<HistoryTypesEntity> resultEntity = await _context.HistoryTypesTable
                .AsNoTracking()
                .ToListAsync(token);
            List<HistoryTypes> result = new List<HistoryTypes>();
            foreach (HistoryTypesEntity h in resultEntity)
            {
                result.Add(MapperEntity.FromHistoryTypesEntity(h));
            }
            return result;
        }

        public async Task<int> CountAsync(CancellationToken token)
        {
            return await _context.HistoryTypesTable
                .CountAsync(token);
        }

        public async Task<List<HistoryTypes>> GetByPaginationAsync(int currentPage, int sizePage,
            CancellationToken token)
        {
            List<HistoryTypesEntity> entities = await _context.HistoryTypesTable
                .AsNoTracking()
                .Skip((currentPage - 1) * sizePage)
                .Take(sizePage)
                .ToListAsync(token);
            List<HistoryTypes> result = new();
            foreach(HistoryTypesEntity h in entities)
            {
                result.Add(MapperEntity.FromHistoryTypesEntity(h));
            }
            return result;
        }
    }
}
