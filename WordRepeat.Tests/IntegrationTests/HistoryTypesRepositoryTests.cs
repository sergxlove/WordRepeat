using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite;
using WordRepeat.DataAccess.Sqlite.Repositories;

namespace WordRepeat.Tests.IntegrationTests
{
    public class HistoryTypesRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly WordRepeatDbContext _context;
        private readonly HistoryTypesRepository _repository;

        public HistoryTypesRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WordRepeatDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WordRepeatDbContext(options);
            _repository = new HistoryTypesRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddHistoryTypes_AndReturnId()
        {
            var id = Guid.NewGuid();
            var createResult = HistoryTypes.Create(id, "Test Type");
            var historyTypes = createResult.Value;
            var resultId = await _repository.AddAsync(historyTypes, CancellationToken.None);
            Assert.Equal(id, resultId);
            var addedEntity = await _context.HistoryTypesTable.FindAsync(id);
            Assert.NotNull(addedEntity);
            Assert.Equal(historyTypes.NameType, addedEntity.NameType);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllHistoryTypes()
        {
            var type1 = HistoryTypes.Create(Guid.NewGuid(), "Type 1").Value;
            var type2 = HistoryTypes.Create(Guid.NewGuid(), "Type 2").Value;
            await _repository.AddAsync(type1, CancellationToken.None);
            await _repository.AddAsync(type2, CancellationToken.None);
            var result = await _repository.GetAllAsync(CancellationToken.None);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.NameType == "Type 1");
            Assert.Contains(result, t => t.NameType == "Type 2");
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            await _repository.AddAsync(HistoryTypes.Create(Guid.NewGuid(), "Type 1").Value, CancellationToken.None);
            await _repository.AddAsync(HistoryTypes.Create(Guid.NewGuid(), "Type 2").Value, CancellationToken.None);
            await _repository.AddAsync(HistoryTypes.Create(Guid.NewGuid(), "Type 3").Value, CancellationToken.None);
            var count = await _repository.CountAsync(CancellationToken.None);
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task CountAsync_WhenNoRecords_ShouldReturnZero()
        {
            var count = await _repository.CountAsync(CancellationToken.None);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetByPaginationAsync_ShouldReturnCorrectPage()
        {
            for (int i = 1; i <= 10; i++)
            {
                var type = HistoryTypes.Create(Guid.NewGuid(), $"Type {i}").Value;
                await _repository.AddAsync(type, CancellationToken.None);
            }
            var page1 = await _repository.GetByPaginationAsync(1, 3, CancellationToken.None);
            var page2 = await _repository.GetByPaginationAsync(2, 3, CancellationToken.None);
            var page3 = await _repository.GetByPaginationAsync(3, 3, CancellationToken.None);
            var page4 = await _repository.GetByPaginationAsync(4, 3, CancellationToken.None);
            Assert.Equal(3, page1.Count);
            Assert.Equal(3, page2.Count);
            Assert.Equal(3, page3.Count);
            Assert.Single(page4);
            Assert.Equal("Type 1", page1[0].NameType);
            Assert.Equal("Type 2", page1[1].NameType);
            Assert.Equal("Type 3", page1[2].NameType);
            Assert.Equal("Type 4", page2[0].NameType);
            Assert.Equal("Type 10", page4[0].NameType);
        }

        [Fact]
        public async Task GetByPaginationAsync_WithPageSizeGreaterThanRecords_ShouldReturnAllRecords()
        {
            for (int i = 1; i <= 5; i++)
            {
                var type = HistoryTypes.Create(Guid.NewGuid(), $"Type {i}").Value;
                await _repository.AddAsync(type, CancellationToken.None);
            }
            var result = await _repository.GetByPaginationAsync(1, 10, CancellationToken.None);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task GetByPaginationAsync_WithEmptyTable_ShouldReturnEmptyList()
        {
            var result = await _repository.GetByPaginationAsync(1, 5, CancellationToken.None);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByPaginationAsync_WithInvalidPageNumber_ShouldReturnEmptyList()
        {
            for (int i = 1; i <= 3; i++)
            {
                var type = HistoryTypes.Create(Guid.NewGuid(), $"Type {i}").Value;
                await _repository.AddAsync(type, CancellationToken.None);
            }
            var result = await _repository.GetByPaginationAsync(10, 3, CancellationToken.None);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
