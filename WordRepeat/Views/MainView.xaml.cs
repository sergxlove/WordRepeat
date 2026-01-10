using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace WordRepeat.Views
{
    public partial class MainView : UserControl
    {
        private ServiceProvider _serviceProvider;
        public MainView(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }
    }
}
