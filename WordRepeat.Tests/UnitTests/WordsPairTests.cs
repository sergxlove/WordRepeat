using WordRepeat.Core.Models;

namespace WordRepeat.Tests.UnitTests
{
    public class WordsPairTests
    {
        [Fact]
        public void Create_WithValidWordAndTranslate_ReturnsSuccessResult()
        {
            string word = "Hello";
            string translate = "Привет";
            var result = WordsPair.Create(word, translate);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
            Assert.Equal(word, result.Value.Word);
            Assert.Equal(translate, result.Value.Translate);
        }

        [Fact]
        public void Create_WithEmptyWord_ReturnsFailureResult()
        {
            string word = "";
            string translate = "Привет";
            var result = WordsPair.Create(word, translate);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("word is null", result.Error);
        }

        [Fact]
        public void Create_WithEmptyTranslate_ReturnsFailureResult()
        {
            string word = "Hello";
            string translate = "";
            var result = WordsPair.Create(word, translate);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("tranclate is null", result.Error);
        }

        [Fact]
        public void Create_WithIdAndValidParameters_ReturnsSuccessResult()
        {
            Guid id = Guid.NewGuid();
            string word = "Hello";
            string translate = "Привет";
            var result = WordsPair.Create(id, word, translate);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(word, result.Value.Word);
            Assert.Equal(translate, result.Value.Translate);
        }

        [Fact]
        public void Create_WithIdAndEmptyWord_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string word = "";
            string translate = "Привет";
            var result = WordsPair.Create(id, word, translate);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("word is null", result.Error);
        }

        [Fact]
        public void Create_WithIdAndEmptyTranslate_ReturnsFailureResult()
        {
            Guid id = Guid.NewGuid();
            string word = "Hello";
            string translate = "";
            var result = WordsPair.Create(id, word, translate);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("tranclate is null", result.Error);
        }
    }
}
