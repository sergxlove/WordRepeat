using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Enums;
using WordRepeat.Models;

namespace WordRepeat.Views
{   
    public partial class TrainResultView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public TrainResultView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _appData.ChangeViewAction(VariableView.Train);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CorrectAnswersText.Text = _appData.TrainResult.CountDone.ToString();
            TotalQuestionsText.Text = _appData.Train.CountWord.ToString();
            PercentageText.Text = (100 / _appData.Train.CountWord * _appData.TrainResult.CountDone)
                .ToString() + "%";
            TimeText.Text = TimeFromSeconds(_appData.TrainResult.TrainingTimeSeconds);
            TimePerQuestionText.Text = (_appData.TrainResult.TrainingTimeSeconds 
                / _appData.Train.CountWord).ToString() + " сек/вопрос";
            CorrectCountText.Text = _appData.TrainResult.CountDone.ToString();
            WrongCountText.Text = (_appData.Train.CountWord - _appData.TrainResult.CountDone).ToString();
            BestStreakText.Text = _appData.TrainResult.Streak.ToString();
        }

        private string TimeFromSeconds(int seconds)
        {
            int minutes = seconds / 60;
            string result = string.Empty;
            if (minutes < 10) result += "0" + minutes.ToString() + ":";
            else result += minutes.ToString() + ":";
            if (seconds % 60 < 10) result += "0" + (seconds % 60).ToString();
            else result += (seconds % 60).ToString();
            return result;
        }
    }
}
