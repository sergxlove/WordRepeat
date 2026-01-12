using WordRepeat.Enums;

namespace WordRepeat.Models
{
    public class AppData
    {
        public Action<VariableView> ChangeViewAction { get; set; } 
        public TrainData Train { get; set; }
        public TrainResultData TrainResult { get; set; }
        public StatsData Stats { get; set; }

        public AppData(Action<VariableView> changeViewAction, TrainData train, TrainResultData trainResult, StatsData stats)
        {
            ChangeViewAction = changeViewAction;
            Train = train;
            TrainResult = trainResult;
            Stats = stats;
        }
    }
}
