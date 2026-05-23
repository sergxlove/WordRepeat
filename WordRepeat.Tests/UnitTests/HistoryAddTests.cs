using WordRepeat.Core.Models;

namespace WordRepeat.Tests.UnitTests
{
    public class HistoryAddTests
    {
        [Fact]
        public void Create_WithValidNameAndCount_ReturnsSuccessResult()
        {
            string name = "Test Name";
            int countAdd = 5;
            var result = HistoryAdd.Create(name, countAdd);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(countAdd, result.Value.CountAdd);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), result.Value.Date);
        }

        [Fact]
        public void Create_WithEmptyName_ReturnsFailureResult()
        {
            string name = "";
            int countAdd = 5;
            var result = HistoryAdd.Create(name, countAdd);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("name is null", result.Error);
        }

        [Fact]
        public void Create_WithIdAndValidParameters_ReturnsSuccessResult()
        {
            Guid id = Guid.NewGuid();
            string name = "Test Name";
            DateOnly date = new DateOnly(2024, 1, 1);
            int countAdd = 10;
            var result = HistoryAdd.Create(id, name, date, countAdd);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(date, result.Value.Date);
            Assert.Equal(countAdd, result.Value.CountAdd);
        }

        [Fact]
        public void Create_WithIdAndEmptyName_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string name = "";
            DateOnly date = new DateOnly(2024, 1, 1);
            int countAdd = 10;
            var result = HistoryAdd.Create(id, name, date, countAdd);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("name is null", result.Error);
        }
    }
}
