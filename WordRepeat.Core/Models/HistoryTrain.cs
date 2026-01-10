using System.ComponentModel;
using WordRepeat.Core.Infrastructures;

namespace WordRepeat.Core.Models
{
    public class HistoryTrain
    {
        [Browsable(false)]
        public Guid Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public int Result { get; set; }
        public int Total { get; set; }
        public DateOnly Date {  get; set; }

        public static ResultCreateModel<HistoryTrain> Create(string name, int result, int total)
        {
            if(string.IsNullOrEmpty(name))
            {
                return ResultCreateModel<HistoryTrain>.Failure("name is null");
            }
            if(total <= 0)
            {
                return ResultCreateModel<HistoryTrain>.Failure("total invalid");
            }

            return ResultCreateModel<HistoryTrain>.Success(new(Guid.NewGuid(), name, result, 
                total, DateOnly.FromDateTime(DateTime.Now)));
        }

        public static ResultCreateModel<HistoryTrain> Create(Guid id, string name, int result, 
            int total, DateOnly date)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ResultCreateModel<HistoryTrain>.Failure("name is null");
            }
            if (total <= 0)
            {
                return ResultCreateModel<HistoryTrain>.Failure("total invalid");
            }

            return ResultCreateModel<HistoryTrain>.Success(new(id, name, result, total, date));
        }

        private HistoryTrain(Guid id, string name, int result, int total, DateOnly date)
        {
            Id = id;
            Name = name;
            Result = result;
            Total = total;
            Date = date;
        }
    }
}
