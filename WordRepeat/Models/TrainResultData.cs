namespace WordRepeat.Models
{
    public class TrainResultData
    {
        public int CountDone { get; set; }
        public int Streak { get; set; }
        public int TrainingTimeSeconds { get; set; }

        public TrainResultData()
        {
            CountDone = 0;
            Streak = 0;
            TrainingTimeSeconds = 0;
        }

        public TrainResultData(int countDone, int streak,  int trainingTime)
        {
            CountDone = countDone;
            Streak = streak;
            TrainingTimeSeconds = trainingTime;
        }
    }
}
