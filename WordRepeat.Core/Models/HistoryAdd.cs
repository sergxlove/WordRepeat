using System.ComponentModel;
using WordRepeat.Core.Infrastructures;

namespace WordRepeat.Core.Models
{
    public class HistoryAdd
    {
        [Browsable(false)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int CountAdd { get; set; }

        public static ResultCreateModel<HistoryAdd> Create(string name, int countAdd)
        {
            if(string.IsNullOrEmpty(name))
            {
                return ResultCreateModel<HistoryAdd>.Failure("name is null");
            }
            
            return ResultCreateModel<HistoryAdd>.Success(new(Guid.NewGuid(), name,
                DateOnly.FromDateTime(DateTime.Now), countAdd));
        }

        public static ResultCreateModel<HistoryAdd> Create(Guid id, string name, DateOnly date, 
            int countAdd)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ResultCreateModel<HistoryAdd>.Failure("name is null");
            }

            return ResultCreateModel<HistoryAdd>.Success(new(id, name, date, countAdd));
        }
        private HistoryAdd(Guid id, string name, DateOnly date, int countAdd)
        {
            Id = id;
            Name = name;
            Date = date;
            CountAdd = countAdd;
        }
    }
}
