using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace WordRepeat.Views
{
    public partial class HistoryView : UserControl
    {
        private ServiceProvider _serviceProvider;
        public HistoryView(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }
    }
}
