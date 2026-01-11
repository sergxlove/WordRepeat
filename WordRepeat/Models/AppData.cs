using WordRepeat.Enums;

namespace WordRepeat.Models
{
    public class AppData
    {
        public int CountWords { get; set; }
        public Action<VariableView> ChangeViewAction { get; set; } 

        public AppData(int countWords, Action<VariableView> changeViewAction)
        {
            CountWords = countWords;
            ChangeViewAction = changeViewAction;
        }

    }
}
