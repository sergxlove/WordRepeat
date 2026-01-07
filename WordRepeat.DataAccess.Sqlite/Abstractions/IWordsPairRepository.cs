using WordRepeat.Core.Models;

namespace WordRepeat.DataAccess.Sqlite.Abstractions
{
    public interface IWordsPairRepository
    {
        Task<Guid> AddAsync(WordsPair wordsPair, CancellationToken token);
        Task<bool> CheckAsync(string word, string translate, CancellationToken token);
        Task<int> DeleteAsync(string word, string translate, CancellationToken token);
        Task<int> UpdateTranslateAsync(string word, string oldTranslate, string newTranslate, CancellationToken token);
        Task<int> UpdateWordAsync(string oldWord, string newWord, string translate, CancellationToken token);
    }
}