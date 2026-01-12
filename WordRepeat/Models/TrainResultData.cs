namespace WordRepeat.Models
{
    public class TrainResultData
    {
        public int CountDone { get; set; }
        public int Streak { get; set; }
        public TimeSpan TrainingTime { get; set; }

        public TrainResultData()
        {
            CountDone = 0;
            Streak = 0;
            TrainingTime = TimeSpan.Zero;
        }

        public TrainResultData(int countDone, int streak,  TimeSpan trainingTime)
        {
            CountDone = countDone;
            Streak = streak;
            TrainingTime = trainingTime;
        }
    }
}
