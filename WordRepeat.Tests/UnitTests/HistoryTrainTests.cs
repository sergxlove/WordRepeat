using WordRepeat.Core.Models;

namespace WordRepeat.Tests.UnitTests
{
    public class HistoryTrainTests
    {
        [Fact]
        public void Create_WithValidParameters_ReturnsSuccessResult()
        {
            string name = "Test Name";
            int result = 8;
            int total = 10;
            var createResult = HistoryTrain.Create(name, result, total);
            Assert.True(createResult.IsSuccess);
            Assert.NotNull(createResult.Value);
            Assert.NotEqual(Guid.Empty, createResult.Value.Id);
            Assert.Equal(name, createResult.Value.Name);
            Assert.Equal(result, createResult.Value.Result);
            Assert.Equal(total, createResult.Value.Total);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), createResult.Value.Date);
        }

        [Fact]
        public void Create_WithEmptyName_ReturnsFailureResult()
        {
            string name = "";
            int result = 8;
            int total = 10;
            var createResult = HistoryTrain.Create(name, result, total);
            Assert.False(createResult.IsSuccess);
            Assert.Null(createResult.Value);
            Assert.Equal("name is null", createResult.Error);
        }

        [Fact]
        public void Create_WithInvalidTotal_ReturnsFailureResult()
        {
            string name = "Test Name";
            int result = 8;
            int total = 0;
            var createResult = HistoryTrain.Create(name, result, total);
            Assert.False(createResult.IsSuccess);
            Assert.Null(createResult.Value);
            Assert.Equal("total invalid", createResult.Error);
        }

        [Fact]
        public void Create_WithNegativeTotal_ReturnsFailureResult()
        {
            string name = "Test Name";
            int result = 8;
            int total = -5;
            var createResult = HistoryTrain.Create(name, result, total);
            Assert.False(createResult.IsSuccess);
            Assert.Null(createResult.Value);
            Assert.Equal("total invalid", createResult.Error);
        }

        [Fact]
        public void Create_WithIdAndValidParameters_ReturnsSuccessResult()
        {
            Guid id = Guid.NewGuid();
            string name = "Test Name";
            int result = 8;
            int total = 10;
            DateOnly date = new DateOnly(2024, 1, 1);
            var createResult = HistoryTrain.Create(id, name, result, total, date);
            Assert.True(createResult.IsSuccess);
            Assert.NotNull(createResult.Value);
            Assert.Equal(id, createResult.Value.Id);
            Assert.Equal(name, createResult.Value.Name);
            Assert.Equal(result, createResult.Value.Result);
            Assert.Equal(total, createResult.Value.Total);
            Assert.Equal(date, createResult.Value.Date);
        }

        [Fact]
        public void Create_WithIdAndEmptyName_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string name = "";
            int result = 8;
            int total = 10;
            DateOnly date = new DateOnly(2024, 1, 1);
            var createResult = HistoryTrain.Create(id, name, result, total, date);
            Assert.False(createResult.IsSuccess);
            Assert.Null(createResult.Value);
            Assert.Equal("name is null", createResult.Error);
        }

        [Fact]
        public void Create_WithIdAndInvalidTotal_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string name = "Test Name";
            int result = 8;
            int total = 0;
            DateOnly date = new DateOnly(2024, 1, 1);
            var createResult = HistoryTrain.Create(id, name, result, total, date);
            Assert.False(createResult.IsSuccess);
            Assert.Null(createResult.Value);
            Assert.Equal("total invalid", createResult.Error);
        }
    }
}
