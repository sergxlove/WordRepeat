using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite;
using WordRepeat.DataAccess.Sqlite.Repositories;

namespace WordRepeat.Tests.IntegrationTests
{
    public class HistoryAddRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly WordRepeatDbContext _context;
        private readonly HistoryAddRepository _repository;

        public HistoryAddRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WordRepeatDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WordRepeatDbContext(options);
            _repository = new HistoryAddRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddHistoryAdd_AndReturnId()
        {
            var createResult = HistoryAdd.Create("Test Name", 5);
            var historyAdd = createResult.Value;
            var id = await _repository.AddAsync(historyAdd, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, id);
            var addedEntity = await _context.HistoryAddTable.FindAsync(id);
            Assert.NotNull(addedEntity);
            Assert.Equal(historyAdd.Name, addedEntity.Name);
            Assert.Equal(historyAdd.CountAdd, addedEntity.CountAdd);
            Assert.Equal(historyAdd.Date, addedEntity.Date);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllHistoryAdds()
        {
            var historyAdd1 = HistoryAdd.Create("Name1", 3).Value;
            var historyAdd2 = HistoryAdd.Create("Name2", 7).Value;
            await _repository.AddAsync(historyAdd1, CancellationToken.None);
            await _repository.AddAsync(historyAdd2, CancellationToken.None);
            var result = await _repository.GetAllAsync(CancellationToken.None);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, h => h.Name == "Name1" && h.CountAdd == 3);
            Assert.Contains(result, h => h.Name == "Name2" && h.CountAdd == 7);
        }

        [Fact]
        public async Task GetByIdAsync_WhenExists_ShouldReturnHistoryAdd()
        {
            var createResult = HistoryAdd.Create("Test Name", 10);
            var historyAdd = createResult.Value;
            var id = await _repository.AddAsync(historyAdd, CancellationToken.None);
            var result = await _repository.GetByIdAsync(id, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(historyAdd.Name, result.Name);
            Assert.Equal(historyAdd.CountAdd, result.CountAdd);
            Assert.Equal(historyAdd.Date, result.Date);
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
            var historyAdd = HistoryAdd.Create("Test Name", 5).Value;
            await _repository.AddAsync(historyAdd, CancellationToken.None);
            var exists = await _repository.CheckByDateAsync(historyAdd.Date, CancellationToken.None);
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckByDateAsync_WhenDateNotExists_ShouldReturnFalse()
        {
            var date = new DateOnly(2025, 1, 1);
            var exists = await _repository.CheckByDateAsync(date, CancellationToken.None);
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateCountAsync_ShouldUpdateCountForDate()
        {
            var historyAdd = HistoryAdd.Create("Test Name", 5).Value;
            await _repository.AddAsync(historyAdd, CancellationToken.None);
            var date = historyAdd.Date;
            var updatedCount = await _repository.UpdateCountAsync(3, date, CancellationToken.None);
            _context.ChangeTracker.Clear();
            Assert.Equal(1, updatedCount);
            var updatedEntity = await _context.HistoryAddTable
                .FirstOrDefaultAsync(h => h.Date == date);
            Assert.NotNull(updatedEntity);
            Assert.Equal(8, updatedEntity.CountAdd);
        }

        [Fact]
        public async Task GetAddedTodayAsync_ShouldReturnSumOfTodayCounts()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var historyAdd1 = HistoryAdd.Create("Name1", 5).Value;
            var historyAdd2 = HistoryAdd.Create("Name2", 3).Value;
            await _repository.AddAsync(historyAdd1, CancellationToken.None);
            await _repository.AddAsync(historyAdd2, CancellationToken.None);
            var totalToday = await _repository.GetAddedTodayAsync(CancellationToken.None);
            Assert.Equal(8, totalToday);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
