using WordRepeat.Core.Models;

namespace WordRepeat.Tests.UnitTests
{
    public class HistoryTypesTests
    {
        [Fact]
        public void Create_WithValidParameters_ReturnsSuccessResult()
        {
            Guid id = Guid.NewGuid();
            string nameType = "Test Type";
            var result = HistoryTypes.Create(id, nameType);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(nameType, result.Value.NameType);
        }

        [Fact]
        public void Create_WithEmptyNameType_ReturnsSuccessResult()
        {
            Guid id = Guid.NewGuid();
            string nameType = "";
            var result = HistoryTypes.Create(id, nameType);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal("", result.Value.NameType);
        }

        [Fact]
        public void Create_WithEmptyGuid_ReturnsSuccessResult()
        {
            Guid id = Guid.Empty;
            string nameType = "Test Type";
            var result = HistoryTypes.Create(id, nameType);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(Guid.Empty, result.Value.Id);
            Assert.Equal(nameType, result.Value.NameType);
        }
    }
}
