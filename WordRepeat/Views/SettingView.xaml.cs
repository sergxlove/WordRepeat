using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace WordRepeat.Views
{
    public partial class SettingView : UserControl
    {
        private ServiceProvider _serviceProvider;
        public SettingView(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private void ImportWordsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportWordsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
