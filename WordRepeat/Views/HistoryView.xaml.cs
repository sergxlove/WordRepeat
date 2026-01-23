using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class HistoryView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        private ObservableCollection<HistoryData> _historyAll;
        private int _currentPage = 1;
        private int _sizePage = 50;
        private int _totalHistory = 0;
        public HistoryView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
            _historyAll = new ObservableCollection<HistoryData>();
            HistoryDataGrid.ItemsSource = _historyAll;
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

        private async void LoadData()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IHistoryTypesService typesService = _serviceProvider
                .GetRequiredService<IHistoryTypesService>();
            IHistoryTrainService trainService = _serviceProvider
                .GetRequiredService<IHistoryTrainService>();
            IHistoryAddServices addService = _serviceProvider
                .GetRequiredService<IHistoryAddServices>();
            List<HistoryTypes> histories = await typesService
                .GetByPaginationAsync(_currentPage, _sizePage, token);
            _historyAll.Clear();
            foreach (HistoryTypes historyType in histories)
            {
                HistoryData hd = new();
                if(historyType.NameType == "add")
                {
                    HistoryAdd? historyAdd = await addService.GetByIdAsync(historyType.Id, token);
                    if(historyAdd != null) hd = HistoryData.Create(historyAdd);
                }
                else if(historyType.NameType == "train")
                {
                    HistoryTrain? historyTrain = await trainService.GetByIdAsync (historyType.Id, token);
                    if(historyTrain != null) hd = HistoryData.Create(historyTrain);
                }
                _historyAll.Add(hd);
            }
            TotalHistoryCount.Text = Convert.ToString(await typesService.CountAsync(token));
            TotalPagesText.Text = GetLastPage().ToString();
        }

        private int GetLastPage()
        {
            if (_totalHistory <= _sizePage) return 1;
            return (int)Math.Ceiling((double)_totalHistory / _sizePage);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
