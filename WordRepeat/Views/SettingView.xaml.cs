using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class SettingView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public SettingView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }

        private void ImportWordsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportWordsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
