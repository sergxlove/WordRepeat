using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite;
using WordRepeat.DataAccess.Sqlite.Repositories;

namespace WordRepeat.Tests.IntegrationTests
{
    public class WordsPairRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly WordRepeatDbContext _context;
        private readonly WordsPairRepository _repository;

        public WordsPairRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WordRepeatDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WordRepeatDbContext(options);
            _repository = new WordsPairRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddWordsPair_AndReturnId()
        {
            var createResult = WordsPair.Create("Hello", "Привет");
            var wordsPair = createResult.Value;
            var id = await _repository.AddAsync(wordsPair, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, id);
            var addedEntity = await _context.WordPairsTable.FindAsync(id);
            Assert.NotNull(addedEntity);
            Assert.Equal(wordsPair.Word, addedEntity.Word);
            Assert.Equal(wordsPair.Translate, addedEntity.Translate);
        }

        [Fact]
        public async Task CheckAsync_WhenPairExists_ShouldReturnTrue()
        {
            var wordsPair = WordsPair.Create("Hello", "Привет").Value;
            await _repository.AddAsync(wordsPair, CancellationToken.None);
            var exists = await _repository.CheckAsync("Hello", "Привет", CancellationToken.None);
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckAsync_WhenPairNotExists_ShouldReturnFalse()
        {
            var exists = await _repository.CheckAsync("Hello", "Привет", CancellationToken.None);
            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteAsync_WhenPairExists_ShouldDeleteAndReturnOne()
        {
            var wordsPair = WordsPair.Create("Hello", "Привет").Value;
            await _repository.AddAsync(wordsPair, CancellationToken.None);
            var deletedCount = await _repository.DeleteAsync("Hello", "Привет", CancellationToken.None);
            Assert.Equal(1, deletedCount);
            var exists = await _repository.CheckAsync("Hello", "Привет", CancellationToken.None);
            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteAsync_WhenPairNotExists_ShouldReturnZero()
        {
            var deletedCount = await _repository.DeleteAsync("Hello", "Привет", CancellationToken.None);
            Assert.Equal(0, deletedCount);
        }

        [Fact]
        public async Task UpdateWordAsync_ShouldUpdateWord()
        {
            var wordsPair = WordsPair.Create("Hello", "Привет").Value;
            await _repository.AddAsync(wordsPair, CancellationToken.None);
            var updatedCount = await _repository.UpdateWordAsync("Hello", "Hi", "Привет", CancellationToken.None);
            Assert.Equal(1, updatedCount);
            var exists = await _repository.CheckAsync("Hi", "Привет", CancellationToken.None);
            Assert.True(exists);
        }

        [Fact]
        public async Task UpdateTranslateAsync_ShouldUpdateTranslate()
        {
            var wordsPair = WordsPair.Create("Hello", "Привет").Value;
            await _repository.AddAsync(wordsPair, CancellationToken.None);
            var updatedCount = await _repository.UpdateTranslateAsync("Hello", "Привет", "Здравствуйте", CancellationToken.None);
            Assert.Equal(1, updatedCount);
            var exists = await _repository.CheckAsync("Hello", "Здравствуйте", CancellationToken.None);
            Assert.True(exists);
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            await _repository.AddAsync(WordsPair.Create("Hello", "Привет").Value, CancellationToken.None);
            await _repository.AddAsync(WordsPair.Create("Goodbye", "До свидания").Value, CancellationToken.None);
            await _repository.AddAsync(WordsPair.Create("Morning", "Утро").Value, CancellationToken.None);
            var count = await _repository.CountAsync(CancellationToken.None);
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetByPaginationAsync_ShouldReturnCorrectPage()
        {
            for (int i = 1; i <= 10; i++)
            {
                var pair = WordsPair.Create($"Word{i}", $"Translate{i}").Value;
                await _repository.AddAsync(pair, CancellationToken.None);
            }
            var page1 = await _repository.GetByPaginationAsync(1, 3, CancellationToken.None);
            var page2 = await _repository.GetByPaginationAsync(2, 3, CancellationToken.None);
            var page4 = await _repository.GetByPaginationAsync(4, 3, CancellationToken.None);
            Assert.Equal(3, page1.Count);
            Assert.Equal(3, page2.Count);
            Assert.Single(page4);
            Assert.Equal("Word1", page1[0].Word);
            Assert.Equal("Word4", page2[0].Word);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
