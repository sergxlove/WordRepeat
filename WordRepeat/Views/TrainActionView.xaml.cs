using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Enums;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class TrainActionView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        private int _currentWord = 1;
        private int _wordDone = 0;
        public TrainActionView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }

        private void LoadData()
        {
            if (_appData.Train.Type == TypeQuestion.Enter)
            {
                OptionPanel.Visibility = Visibility.Collapsed;
                InputPanel.Visibility = Visibility.Visible;
            }
            else if (_appData.Train.Type == TypeQuestion.Select)
            {
                OptionPanel.Visibility = Visibility.Visible;
                InputPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AnswerTextBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            _appData.ChangeViewAction(VariableView.Train);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
