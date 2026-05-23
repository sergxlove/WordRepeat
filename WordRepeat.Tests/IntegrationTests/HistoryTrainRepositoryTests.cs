using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite;
using WordRepeat.DataAccess.Sqlite.Models;
using WordRepeat.DataAccess.Sqlite.Repositories;

namespace WordRepeat.Tests.IntegrationTests
{
    public class HistoryTrainRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly WordRepeatDbContext _context;
        private readonly HistoryTrainRepository _repository;

        public HistoryTrainRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WordRepeatDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WordRepeatDbContext(options);
            _repository = new HistoryTrainRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddHistoryTrain_AndReturnId()
        {
            var createResult = HistoryTrain.Create("Test Name", 8, 10);
            var historyTrain = createResult.Value;
            var id = await _repository.AddAsync(historyTrain, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, id);
            var addedEntity = await _context.HistoryTrainTable.FindAsync(id);
            Assert.NotNull(addedEntity);
            Assert.Equal(historyTrain.Name, addedEntity.Name);
            Assert.Equal(historyTrain.Result, addedEntity.Result);
            Assert.Equal(historyTrain.Total, addedEntity.Total);
            Assert.Equal(historyTrain.Date, addedEntity.Date);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllHistoryTrains()
        {
            var historyTrain1 = HistoryTrain.Create("Name1", 5, 10).Value;
            var historyTrain2 = HistoryTrain.Create("Name2", 7, 10).Value;
            await _repository.AddAsync(historyTrain1, CancellationToken.None);
            await _repository.AddAsync(historyTrain2, CancellationToken.None);
            var result = await _repository.GetAllAsync(CancellationToken.None);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_WhenExists_ShouldReturnHistoryTrain()
        {
            var historyTrain = HistoryTrain.Create("Test Name", 8, 10).Value;
            var id = await _repository.AddAsync(historyTrain, CancellationToken.None);
            var result = await _repository.GetByIdAsync(id, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(historyTrain.Name, result.Name);
            Assert.Equal(historyTrain.Result, result.Result);
            Assert.Equal(historyTrain.Total, result.Total);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckByDateAsync_WhenDateExists_ShouldReturnTrue()
        {
            var historyTrain = HistoryTrain.Create("Test Name", 8, 10).Value;
            await _repository.AddAsync(historyTrain, CancellationToken.None);
            var exists = await _repository.CheckByDateAsync(historyTrain.Date, CancellationToken.None);
            Assert.True(exists);
        }

        [Fact]
        public async Task UpdateCountAsync_ShouldUpdateResultAndTotal()
        {
            var historyTrain = HistoryTrain.Create("Test Name", 5, 10).Value;
            await _repository.AddAsync(historyTrain, CancellationToken.None);
            var date = historyTrain.Date;
            var updatedCount = await _repository.UpdateCountAsync(3, 5, date, CancellationToken.None);
            _context.ChangeTracker.Clear();
            Assert.Equal(1, updatedCount);
            var updatedEntity = await _context.HistoryTrainTable
                .FirstOrDefaultAsync(h => h.Date == date);
            Assert.NotNull(updatedEntity);
            Assert.Equal(8, updatedEntity.Result);
            Assert.Equal(15, updatedEntity.Total);
        }

        [Fact]
        public async Task GetTrainedTodayAsync_ShouldReturnSumOfTodayTotals()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var historyTrain1 = HistoryTrain.Create("Name1", 5, 10).Value;
            var historyTrain2 = HistoryTrain.Create("Name2", 3, 7).Value;
            await _repository.AddAsync(historyTrain1, CancellationToken.None);
            await _repository.AddAsync(historyTrain2, CancellationToken.None);
            var totalToday = await _repository.GetTrainedTodayAsync(CancellationToken.None);
            Assert.Equal(17, totalToday);
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            await _repository.AddAsync(HistoryTrain.Create("Name1", 5, 10).Value, CancellationToken.None);
            await _repository.AddAsync(HistoryTrain.Create("Name2", 3, 7).Value, CancellationToken.None);
            var count = await _repository.CountAsync(CancellationToken.None);
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetCountDoneAsync_ShouldReturnSumOfResults()
        {
            await _repository.AddAsync(HistoryTrain.Create("Name1", 5, 10).Value, CancellationToken.None);
            await _repository.AddAsync(HistoryTrain.Create("Name2", 3, 7).Value, CancellationToken.None);
            var done = await _repository.GetCountDoneAsync(CancellationToken.None);
            Assert.Equal(8, done);
        }

        [Fact]
        public async Task GetCountWrongAsync_ShouldReturnDifferenceBetweenTotalAndResult()
        {
            await _repository.AddAsync(HistoryTrain.Create("Name1", 5, 10).Value, CancellationToken.None);
            await _repository.AddAsync(HistoryTrain.Create("Name2", 3, 7).Value, CancellationToken.None);
            var wrong = await _repository.GetCountWrongAsync(CancellationToken.None);
            Assert.Equal(9, wrong);
        }

        [Fact]
        public async Task GetAccuracyByAllAsync_WhenTotalIsZero_ShouldReturnZero()
        {
            var accuracy = await _repository.GetAccuracyByAllAsync(CancellationToken.None);
            Assert.Equal(0, accuracy);
        }

        [Fact]
        public async Task GetStreakAsync_ShouldReturnConsecutiveDaysCount()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var yesterday = today.AddDays(-1);
            var twoDaysAgo = today.AddDays(-2);
            var threeDaysAgo = today.AddDays(-3);
            await _repository.AddAsync(HistoryTrain.Create("Name1", 5, 10).Value, CancellationToken.None);
            var historyYesterday = HistoryTrain.Create("Name2", 3, 8).Value;
            var historyYesterdayEntity = new
            {
                Id = Guid.NewGuid(),
                Name = historyYesterday.Name,
                Result = historyYesterday.Result,
                Total = historyYesterday.Total,
                Date = yesterday
            };
            await _context.HistoryTrainTable.AddAsync(new HistoryTrainEntity
            {
                Id = historyYesterdayEntity.Id,
                Name = historyYesterdayEntity.Name,
                Result = historyYesterdayEntity.Result,
                Total = historyYesterdayEntity.Total,
                Date = historyYesterdayEntity.Date
            });
            var historyTwoDaysAgo = HistoryTrain.Create("Name3", 4, 9).Value;
            var historyTwoDaysAgoEntity = new
            {
                Id = Guid.NewGuid(),
                Name = historyTwoDaysAgo.Name,
                Result = historyTwoDaysAgo.Result,
                Total = historyTwoDaysAgo.Total,
                Date = twoDaysAgo
            };
            await _context.HistoryTrainTable.AddAsync(new HistoryTrainEntity
            {
                Id = historyTwoDaysAgoEntity.Id,
                Name = historyTwoDaysAgoEntity.Name,
                Result = historyTwoDaysAgoEntity.Result,
                Total = historyTwoDaysAgoEntity.Total,
                Date = historyTwoDaysAgoEntity.Date
            });
            await _context.SaveChangesAsync();
            var streak = await _repository.GetStreakAsync(CancellationToken.None);
            Assert.Equal(3, streak);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
