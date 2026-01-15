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
                .FirstOrDefaultAsync(a => a.Word == word && a.Translate == translate, token);
            if (result is null) return false;
            return true;
        }

        public async Task<int> DeleteAsync(string word, string translate, CancellationToken token)
        {
            return await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word == word && a.Translate == translate)
                .ExecuteDeleteAsync(token);
        }

        public async Task<int> UpdateWordAsync(string oldWord, string newWord,
            string translate, CancellationToken token)
        {
            return await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word == oldWord && a.Translate == translate)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Word, newWord), token);
        }

        public async Task<int> UpdateTranslateAsync(string word, string oldTranslate,
            string newTranslate, CancellationToken token)
        {
            return await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word == word && a.Translate == oldTranslate)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Translate, newTranslate), token);
        }

        public async Task<List<WordsPair>> GetByPaginationAsync(int currentPage, int sizePage, 
            CancellationToken token)
        {
            List<WordsPairEntity> entities = await _context.WordPairsTable
                .AsNoTracking()
                .Skip((currentPage - 1) * sizePage)
                .Take(sizePage)
                .ToListAsync(token);
            List<WordsPair> result = new List<WordsPair>();
            foreach (WordsPairEntity e in entities)
            {
                result.Add(MapperEntity.FromWordsPairEntity(e));
            }
            return result;
        }

        public async Task<int> CountAsync(CancellationToken token)
        {
            return await _context.WordPairsTable
                .CountAsync(token);
        }

        public async Task<List<WordsPair>> GetByWordAsync(string word, CancellationToken token)
        {
            List<WordsPairEntity> entities = await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Word.Contains(word.ToLower().Trim()))
                .ToListAsync(token);
            List<WordsPair> result = new List<WordsPair>();
            foreach(WordsPairEntity e in entities)
            {
                result.Add(MapperEntity.FromWordsPairEntity(e));
            }
            return result;
        }

        public async Task<List<WordsPair>> GetByTranslateAsync(string translate, CancellationToken token)
        {
            List<WordsPairEntity> entities = await _context.WordPairsTable
                .AsNoTracking()
                .Where(a => a.Translate.Contains(translate.ToLower().Trim()))
                .ToListAsync(token);
            List<WordsPair> result = new List<WordsPair>();
            foreach (WordsPairEntity e in entities)
            {
                result.Add(MapperEntity.FromWordsPairEntity(e));
            }
            return result;
        }

        public async Task<WordsPair> GetByPositionAsync(int position, CancellationToken token)
        {
            WordsPairEntity result = await _context.WordPairsTable
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .ElementAtAsync(position, token);
            return MapperEntity.FromWordsPairEntity(result);
        }

        public async Task<string> GetTranslateByPositionAsync(int position, CancellationToken token)
        {
            WordsPairEntity result = await _context.WordPairsTable
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .ElementAtAsync(position, token);
            return result.Translate;
        }

        public async Task<string> GetWordByPositionAsync(int position, CancellationToken token)
        {
            WordsPairEntity result = await _context.WordPairsTable
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .ElementAtAsync(position, token);
            return result.Word;
        }
    }
}
