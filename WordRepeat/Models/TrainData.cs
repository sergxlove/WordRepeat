namespace WordRepeat.Models
{
    public class TrainData
    {
        public ModeTrain Mode { get; set; } 
        public TypeQuestion Type { get; set; }
        public int CountWord { get; set; } 
        public bool IsTime { get; set; }

        public TrainData()
        {
            Mode = ModeTrain.None;
            Type = TypeQuestion.None;
            CountWord = 0;
            IsTime = false;
        }

        public TrainData(ModeTrain mode, TypeQuestion type, int countWord, bool isTime)
        {
            Mode = mode;
            Type = type;
            CountWord = countWord;
            IsTime = isTime;
        }
    }


    public enum ModeTrain
    {
        None = 0, 
        WordToTranslate = 1,
        TranslateToWord = 2,
        Mixed = 3
    }

    public enum TypeQuestion
    {
        None = 0,
        Enter = 1,
        Select = 2
    }
}
