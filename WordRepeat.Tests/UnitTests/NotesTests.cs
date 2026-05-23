using WordRepeat.Core.Models;

namespace WordRepeat.Tests.UnitTests
{
    public class NotesTests
    {
        [Fact]
        public void Create_WithValidTitleAndContent_ReturnsSuccessResult()
        {
            string title = "Test Title";
            string content = "Test Content";
            var result = Notes.Create(title, content);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
            Assert.Equal(title, result.Value.Title);
            Assert.Equal(content, result.Value.Content);
            Assert.Equal(DateTime.UtcNow.Date, result.Value.DateUpdate.Date);
        }

        [Fact]
        public void Create_WithEmptyTitle_ReturnsFailureResult()
        {
            string title = "";
            string content = "Test Content";
            var result = Notes.Create(title, content);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("title is null", result.Error);
        }

        [Fact]
        public void Create_WithEmptyContent_ReturnsFailureResult()
        {
            string title = "Test Title";
            string content = "";
            var result = Notes.Create(title, content);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("content is null", result.Error);
        }

        [Fact]
        public void Create_WithIdAndValidParameters_ReturnsSuccessResult()
        {
            Guid id = Guid.NewGuid();
            string title = "Test Title";
            string content = "Test Content";
            DateTime dateUpdate = new DateTime(2024, 1, 1, 10, 30, 0);
            var result = Notes.Create(id, title, content, dateUpdate);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(title, result.Value.Title);
            Assert.Equal(content, result.Value.Content);
            Assert.Equal(dateUpdate, result.Value.DateUpdate);
        }

        [Fact]
        public void Create_WithIdAndEmptyTitle_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string title = "";
            string content = "Test Content";
            DateTime dateUpdate = DateTime.UtcNow;
            var result = Notes.Create(id, title, content, dateUpdate);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("title is null", result.Error);
        }

        [Fact]
        public void Create_WithIdAndEmptyContent_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string title = "Test Title";
            string content = "";
            DateTime dateUpdate = DateTime.UtcNow;
            var result = Notes.Create(id, title, content, dateUpdate);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("content is null", result.Error);
        }
    }
}
