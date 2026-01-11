using WordRepeat.Core.Infrastructures;
using WordRepeat.Core.Models;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Infrastructures
{
    public class MapperEntity
    {
        public static HistoryAddEntity ToHistoryAddEntity(HistoryAdd historyItem)
        {
            HistoryAddEntity historyAddEntity = new HistoryAddEntity()
            {
                Id = historyItem.Id,
                Name = historyItem.Name,
                CountAdd = historyItem.CountAdd,
                Date = historyItem.Date,
            };
            return historyAddEntity;
        }

        public static HistoryTrainEntity ToHistoryTrainEntity(HistoryTrain historyTrain)
        {
            HistoryTrainEntity historyTrainEntity = new HistoryTrainEntity()
            {
                Id = historyTrain.Id,
                Name = historyTrain.Name,
                Date = historyTrain.Date,
                Result = historyTrain.Result,
                Total = historyTrain.Total
            };
            return historyTrainEntity;
        }

        public static HistoryTypesEntity ToHistoryTypesEntity(HistoryTypes historyTypes)
        {
            HistoryTypesEntity historyTypesEntity = new HistoryTypesEntity()
            {
                Id = historyTypes.Id,
                NameType = historyTypes.NameType,
            };
            return historyTypesEntity;
        }

        public static WordsPairEntity ToWordsPairEntity(WordsPair wordsPair)
        {
            WordsPairEntity wordsPairEntity = new WordsPairEntity()
            {
                Id = wordsPair.Id,
                Translate = wordsPair.Translate,
                Word = wordsPair.Word
            };
            return wordsPairEntity;
        }

        public static HistoryAdd FromHistoryAddEntity(HistoryAddEntity h)
        {
            ResultCreateModel<HistoryAdd> historyAdd = HistoryAdd.Create(h.Id, h.Name, 
                h.Date, h.CountAdd);
            if(!string.IsNullOrEmpty(historyAdd.Error))
                throw new Exception(historyAdd.Error);
            return historyAdd.Value;
        }

        public static HistoryTrain FromHistoryTrainEntity(HistoryTrainEntity h)
        {
            ResultCreateModel<HistoryTrain> historyTrain = HistoryTrain.Create(h.Id,
                h.Name, h.Result, h.Total, h.Date);
            if(!string.IsNullOrEmpty(historyTrain.Error))
                throw new Exception(historyTrain.Error);
            return historyTrain.Value;
        }

        public static HistoryTypes FromHistoryTypesEntity(HistoryTypesEntity h)
        {
            ResultCreateModel<HistoryTypes> historyTypes = HistoryTypes.Create(h.Id, h.NameType);
            if(!string.IsNullOrEmpty(historyTypes.Error))
                throw new Exception(historyTypes.Error);
            return historyTypes.Value;
        }

        public static WordsPair FromWordsPairEntity(WordsPairEntity w)
        {
            ResultCreateModel<WordsPair> wordPair = WordsPair.Create(w.Id, w.Word, w.Translate);
            if(!string.IsNullOrEmpty(wordPair.Error))
                throw new Exception(wordPair.Error);
            return wordPair.Value;
        }
    }
}
