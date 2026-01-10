using System.ComponentModel;
using WordRepeat.Core.Infrastructures;

namespace WordRepeat.Core.Models
{
    public class WordsPair
    {
        [Browsable(false)]
        public Guid Id { get; set; } 
        public string Word { get; set; } = string.Empty;
        public string Tranclate { get; set; } = string.Empty;

        public static ResultCreateModel<WordsPair> Create(string word, string tranclate)
        {
            if(string.IsNullOrEmpty(word))
            {
                return ResultCreateModel<WordsPair>.Failure("word is null");
            }
            if (string.IsNullOrEmpty(tranclate))
            {
                return ResultCreateModel<WordsPair>.Failure("tranclate is null");
            }
            return ResultCreateModel<WordsPair>.Success(new(Guid.NewGuid(), word, tranclate));
        }

        public static ResultCreateModel<WordsPair> Create(Guid id, string word, string tranclate)
        {
            if (string.IsNullOrEmpty(word))
            {
                return ResultCreateModel<WordsPair>.Failure("word is null");
            }
            if (string.IsNullOrEmpty(tranclate))
            {
                return ResultCreateModel<WordsPair>.Failure("tranclate is null");
            }
            return ResultCreateModel<WordsPair>.Success(new(id, word, tranclate));
        }
        private WordsPair(Guid id, string word, string tranclate)
        {
            Id = id;
            Word = word;
            Tranclate = tranclate;
        }
    }
}
