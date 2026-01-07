using Microsoft.EntityFrameworkCore;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Infrastructures;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Repositories
{
    public class WordsPairRepository : IWordsPairRepository
    {
        private readonly WordRepeatDbContext _context;
        public WordsPairRepository(WordRepeatDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(WordsPair wordsPair, CancellationToken token)
        {
            WordsPairEntity wordspairEntity = MapperEntity.ToWordsPairEntity(wordsPair);
            await _context.WordPairsTable.AddAsync(wordspairEntity, token);
            await _context.SaveChangesAsync(token);
            return wordspairEntity.Id;
        }

        public async Task<bool> CheckAsync(string word, string translate, CancellationToken token)
        {
            WordsPairEntity? result = await _context.WordPairsTable
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Word == word && a.Tranclate == translate, token);
            if (result is null) return false;
            return true;
        }

        public async Task<int> DeleteAsync(string word, string translate, CancellationToken token)
        {
            return await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word == word && a.Tranclate == translate)
                .ExecuteDeleteAsync(token);
        }

        public async Task<int> UpdateWordAsync(string oldWord, string newWord,
            string translate, CancellationToken token)
        {
            return await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word == oldWord && a.Tranclate == translate)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Word, newWord), token);
        }

        public async Task<int> UpdateTranslateAsync(string word, string oldTranslate,
            string newTranslate, CancellationToken token)
        {
            return await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word == word && a.Tranclate == oldTranslate)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Tranclate, newTranslate), token);
        }
    }
}
