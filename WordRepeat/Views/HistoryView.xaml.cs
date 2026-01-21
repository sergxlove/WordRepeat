using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class HistoryView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        private int _currentPage = 1;
        private int _sizePage = 50;
        private int _totalHistory = 0;
        public HistoryView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
           
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            LoadData();
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage != 1)
            {
                _currentPage -= 1;
            }
            LoadData();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < GetLastPage())
            {
                _currentPage += 1;
            }
            LoadData();
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = GetLastPage();
            LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {

        }

        private int GetLastPage()
        {
            if (_totalHistory <= _sizePage) return 1;
            return (int)Math.Ceiling((double)_totalHistory / _sizePage);
        }
    }
}
