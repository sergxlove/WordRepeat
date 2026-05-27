namespace WordRepeat.Models
{
    public class DailyActivityModel
    {
        public string Day { get; set; } = string.Empty;
        public int AddedWords { get; set; }  
        public int RepeatedWords { get; set; }  
        public double Accuracy { get; set; }  
        public System.Windows.Media.Brush? AccuracyColor { get; set; } 
    }
}
