namespace WordRepeat.Models
{
    public class StatsData
    {
        public int CountWords { get; set; }
        public DateOnly LastTrain { get; set; }
        public string LastTrainResult { get; set; } = string.Empty;
        public int AverageTrain { get; set; }
        public int AddToday { get; set; }
        public int TrainToday { get; set; }

        public StatsData()
        {
            CountWords = 0;
            LastTrain = new DateOnly();
            LastTrainResult = string.Empty;
            AverageTrain = 0;
            AddToday = 0;
            TrainToday = 0;
        }
    }
}
