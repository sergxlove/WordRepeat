using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;

namespace WordRepeat.Application.Services
{
    public class WordPairService : IWordPairService
    {
        private readonly IWordsPairRepository _repository;
        public WordPairService(IWordsPairRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> AddAsync(WordsPair wordsPair, CancellationToken token)
        {
            return await _repository.AddAsync(wordsPair, token);
        }
        public async Task<bool> CheckAsync(string word, string translate, CancellationToken token)
        {
            return await _repository.CheckAsync(word, translate, token);
        }
        public async Task<int> DeleteAsync(string word, string translate, CancellationToken token)
        {
            return await _repository.DeleteAsync(word, translate, token);
        }
        public async Task<int> UpdateTranslateAsync(string word, string oldTranslate,
            string newTranslate, CancellationToken token)
        {
            return await _repository.UpdateTranslateAsync(word, oldTranslate, newTranslate, token);
        }
        public async Task<int> UpdateWordAsync(string oldWord, string newWord, string translate,
            CancellationToken token)
        {
            return await _repository.UpdateWordAsync(oldWord, newWord, translate, token);
        }
        
        public async Task<List<WordsPair>> GetByPaginationAsync(int currentPage, int sizePage,
            CancellationToken token)
        {
            return await _repository.GetByPaginationAsync(currentPage, sizePage, token);
        }

        public async Task<int> CountAsync(CancellationToken token)
        {
            return await _repository.CountAsync(token);
        }

        public async Task<List<WordsPair>> GetByWordAsync(string word, CancellationToken token)
        {
            return await _repository.GetByWordAsync(word, token);
        }
        public async Task<List<WordsPair>> GetByTranslateAsync(string translate, CancellationToken token)
        {
            return await _repository.GetByTranslateAsync(translate, token);
        }
    }
}
