using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Abstractions;
using WordRepeat.Enums;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class TrainView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public TrainView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }
        
        private void StartTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(_appData.Stats.CountWords < 100)
                {
                    INotificationService notification = _serviceProvider
                        .GetRequiredService<INotificationService>();
                    notification.ShowError("Для тренировки необходимо иметь 100 слов в базе");
                    return;
                }
                if (ModeWordRadio.IsChecked == true)
                    _appData.Train.Mode = ModeTrain.WordToTranslate;
                else if (ModeTranslationRadio.IsChecked == true)
                    _appData.Train.Mode = ModeTrain.TranslateToWord;
                else if (ModeMixedRadio.IsChecked == true)
                    _appData.Train.Mode = ModeTrain.Mixed;
                _appData.Train.CountWord = Convert.ToInt32(SelectedCountText.Text);
                if (TypeInputRadio.IsChecked == true)
                    _appData.Train.Type = TypeQuestion.Enter;
                else if (TypeOptionsRadio.IsChecked == true)
                    _appData.Train.Type = TypeQuestion.Select;
                if (ShowTimerCheckBox.IsChecked == true)
                    _appData.Train.IsTime = true;

                _appData.ChangeViewAction(VariableView.TrainAction);
            }
            catch
            {
                INotificationService notification = _serviceProvider
                    .GetRequiredService<INotificationService>();
                notification.ShowError("Произошла ошибка");
            }
        }

        private void WordsCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SelectedCountText is null)
                return;
            SelectedCountText.Text = Math.Ceiling(WordsCountSlider.Value).ToString();
        }
    }
}
