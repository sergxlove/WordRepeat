using WordRepeat.Core.Models;

namespace WordRepeat.Models
{
    public class HistoryData
    {
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; }
        public string Result { get; set; } = string.Empty;

        public HistoryData()
        {
            Title = "Какое-то событие ...";
            Date = "Когда-то ...";
            Result = "Что-то было ..."; 
        }

        public static HistoryData Create(HistoryAdd historyAdd)
        {
            HistoryData result = new();
            result.Title = "Добавление новых слов";
            result.Date = historyAdd.Date.ToString();
            result.Result = $"Добавлено слов: {historyAdd.CountAdd}";
            return result;
        }

        public static HistoryData Create(HistoryTrain historyTrain)
        {
            HistoryData result = new();
            result.Title = "Тренировка";
            result.Date = historyTrain.Date.ToString();
            result.Result = $"{historyTrain.Result} из {historyTrain.Total}";
            return result;
        }
    }
}
