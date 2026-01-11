using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class TrainActionView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public TrainActionView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
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

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
