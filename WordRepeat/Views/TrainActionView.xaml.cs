using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace WordRepeat.Views
{
    public partial class TrainActionView : UserControl
    {
        private ServiceProvider _serviceProvider;
        public TrainActionView(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
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
