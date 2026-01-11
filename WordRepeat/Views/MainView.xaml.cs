using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class MainView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public MainView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }
    }
}
