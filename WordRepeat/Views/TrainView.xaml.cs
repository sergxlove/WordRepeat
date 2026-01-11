using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
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
            _appData.ChangeViewAction(VariableView.TrainAction);
        }
    }
}
