using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class HistoryView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public HistoryView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }
    }
}
