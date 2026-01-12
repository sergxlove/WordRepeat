using WordRepeat.Core.Models;

namespace WordRepeat.Application.Abstractions
{
    public interface IWordPairService
    {
        Task<Guid> AddAsync(WordsPair wordsPair, CancellationToken token);
        Task<bool> CheckAsync(string word, string translate, CancellationToken token);
        Task<int> DeleteAsync(string word, string translate, CancellationToken token);
        Task<int> UpdateTranslateAsync(string word, string oldTranslate, string newTranslate, CancellationToken token);
        Task<int> UpdateWordAsync(string oldWord, string newWord, string translate, CancellationToken token);
        Task<List<WordsPair>> GetByPaginationAsync(int currentPage, int sizePage, CancellationToken token);
        Task<int> CountAsync(CancellationToken token);
        Task<List<WordsPair>> GetByWordAsync(string word, CancellationToken token);
        Task<List<WordsPair>> GetByTranslateAsync(string translate, CancellationToken token);
        Task<WordsPair> GetByPositionAsync(int position, CancellationToken token);
    }
}