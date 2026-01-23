using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Infrastructures;
using WordRepeat.DataAccess.Sqlite.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<HistoryTrain?> GetByIdAsync(Guid id, CancellationToken token)
        {
            HistoryTrainEntity? resultEntity = await _context.HistoryTrainTable
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id  == id, token);
            if (resultEntity is null) return null;
            return MapperEntity.FromHistoryTrainEntity(resultEntity);
        }

        public async Task<int> GetTrainedTodayAsync(CancellationToken token)
        {
            DateOnly dateToday = DateOnly.FromDateTime(DateTime.Now);
            int result = await _context.HistoryTrainTable
                .AsNoTracking()
                .Where(a => a.Date == dateToday)
                .SumAsync(a => a.Total, token);
            return result;
        }

        public async Task<int> GetAccuracyByWeekAsync(CancellationToken token)
        {
            DateOnly dateToday = DateOnly.FromDateTime(DateTime.Now);
            DateOnly weekStart = dateToday.AddDays(-(int)dateToday.DayOfWeek);
            DateOnly weekEnd = weekStart.AddDays(6);
            int result = await _context.HistoryTrainTable
                .AsNoTracking()
                .Where(a => a.Date >=  weekStart && a.Date <= weekEnd)
                .SumAsync(a => a.Result, token);
            int total = await _context.HistoryTrainTable
                .AsNoTracking()
                .Where(a => a.Date >= weekStart && a.Date <= weekEnd)
                .SumAsync(a => a.Total, token);
            double accuracy = result / total * 100;
            return Convert.ToInt32(accuracy);
        }

        public async Task<int> GetAccuracyByAllAsync(CancellationToken token)
        {
            int result = await _context.HistoryTrainTable
                .AsNoTracking()
                .SumAsync(a => a.Result, token);
            int total = await _context.HistoryTrainTable
                .AsNoTracking()
                .SumAsync(a => a.Total, token);
            double accuracy = result / total * 100;
            return Convert.ToInt32(accuracy);
        }

        public async Task<int> GetAccuracyByMonthAsync(CancellationToken token)
        {
            DateOnly dateToday = DateOnly.FromDateTime(DateTime.Now);
            DateOnly monthStart = dateToday.AddDays(-(int)dateToday.DayOfWeek);
            DateOnly monthEnd = monthStart.AddDays(29);
            int result = await _context.HistoryTrainTable
                .AsNoTracking()
                .Where(a => a.Date >= monthStart && a.Date <= monthEnd)
                .SumAsync(a => a.Result, token);
            int total = await _context.HistoryTrainTable
                .AsNoTracking()
                .Where(a => a.Date >= monthStart && a.Date <= monthEnd)
                .SumAsync(a => a.Total, token);
            double accuracy = result / total * 100;
            return Convert.ToInt32(accuracy);
        }

        public async Task<int> CountAsync(CancellationToken token)
        {
            return await _context.HistoryTrainTable
                .AsNoTracking()
                .CountAsync(token);
        }

        public async Task<int> GetCountWrongAsync(CancellationToken token)
        {
            int result = await _context.HistoryTrainTable
                .AsNoTracking()
                .SumAsync(a => a.Result, token);
            int total = await _context.HistoryTrainTable
                .AsNoTracking()
                .SumAsync(a => a.Total, token);
            return total - result;
        }

        public async Task<int> GetCountDoneAsync(CancellationToken token)
        {
            return await _context.HistoryTrainTable
                .AsNoTracking()
                .SumAsync(a => a.Result, token);
        }

        public async Task<int> GetStreakAsync(CancellationToken token)
        {
            DateOnly dateToday = DateOnly.FromDateTime(DateTime.Now);
            int result = 0;
            bool isActive = await _context.HistoryTrainTable
                .AnyAsync(a => a.Date == dateToday, token);
            if (isActive) result++;
            dateToday = dateToday.AddDays(-1);
            isActive = await _context.HistoryTrainTable
                .AnyAsync(a => a.Date == dateToday, token);
            if (isActive) result++;
            else return result;
            dateToday = dateToday.AddDays(-1);
            while (true)
            {
                isActive = await _context.HistoryTrainTable
                   .AnyAsync(a => a.Date == dateToday, token);
                if (isActive)
                {
                    result++;
                    dateToday = dateToday.AddDays(-1);
                }
                else
                {
                    return result;
                }
            }
        }
    }
}
