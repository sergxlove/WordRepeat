using System.ComponentModel;
using WordRepeat.Core.Infrastructures;

namespace WordRepeat.Core.Models
{
    public class Notes
    {
        [Browsable(false)]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateUpdate { get; set; }

        public static ResultCreateModel<Notes> Create(string title, string content)
        {
            return Create(Guid.NewGuid(), title, content, DateTime.UtcNow);
        }

        public static ResultCreateModel<Notes> Create(Guid id, string title, string content,
            DateTime dateUpdate)
        {
            if (string.IsNullOrEmpty(title))
            {
                return ResultCreateModel<Notes>.Failure("title is null");
            }
            if (string.IsNullOrEmpty(content))
            {
                return ResultCreateModel<Notes>.Failure("content is null");
            }
            return ResultCreateModel<Notes>.Success(new Notes(id, title, content, dateUpdate));
        }

        private Notes(Guid id, string title, string content, DateTime dateUpdate)
        {
            Id = id;
            Title = title;
            Content = content;
            DateUpdate = dateUpdate;
        }
    }
}
