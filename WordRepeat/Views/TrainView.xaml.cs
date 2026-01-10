using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace WordRepeat.Views
{
    public partial class TrainView : UserControl
    {
        private ServiceProvider _serviceProvider;
        public TrainView(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }
        
        private void StartTrainingButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
