using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Abstractions;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Infrastructures;
using WordRepeat.Core.Models;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class WordsView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private ObservableCollection<WordsPair> _wordsPairAll;
        private int _totalWords;
        private int _currentPage = 1;
        private int _sizePage = 50;
        private AppData _appData;
        public WordsView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _wordsPairAll = new ObservableCollection<WordsPair>();
            _appData = appData; 
            WordsDataGrid.ItemsSource = _wordsPairAll;
            _sizePage = Convert.ToInt32(PageSizeComboBox.Text);
            LoadData();
        }

        private async void LoadData()
        {
            _wordsPairAll.Clear();
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            _totalWords = await wordService.CountAsync(token);
            List<WordsPair> result = await wordService
                .GetByPaginationAsync(_currentPage, _sizePage, token);
            foreach (WordsPair w in result)
            {
                _wordsPairAll.Add(w);
            }
            TotalWordsCount.Text = _totalWords.ToString();
            TotalPagesText.Text = GetLastPage().ToString();
            CountPagesText.Text = _currentPage.ToString();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            string word = WordTextBox.Text.ToLower().Trim();
            string translate = TranslationTextBox.Text.ToLower().Trim();
            INotificationService notification = _serviceProvider
                .GetRequiredService<INotificationService>();
            if (string.IsNullOrEmpty(word))
            {
                notification.ShowError("Необходимо ввести слово");
            }
            if (string.IsNullOrEmpty(translate))
            {
                notification.ShowError("Необходимо ввести перевод");
            }
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            ResultCreateModel<WordsPair> wordPair = WordsPair.Create(word, translate);
            if(!string.IsNullOrEmpty(wordPair.Error))
            {
                notification.ShowError(wordPair.Error);
                return;
            }
            if(await wordService.CheckAsync(word, translate, token))
            {
                notification.ShowError("Данная пара уже добавлена");
                return;
            }
            Guid result = await wordService.AddAsync(wordPair.Value, token);
            notification.ShowSuccess("Пара успешно добавлена");
            _totalWords++;
            WordTextBox.Text = "";
            TranslationTextBox.Text = "";
            TotalWordsCount.Text = _totalWords.ToString();
        }

        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            string textSearch = SearchTextBox.Text;
            INotificationService notification = _serviceProvider
                .GetRequiredService<INotificationService>();
            IWordPairService wordService = _serviceProvider
                .GetRequiredService<IWordPairService>();
            if (string.IsNullOrEmpty(textSearch))
            {
                notification.ShowError("Строка поиска пуста");
                return;
            }
            SearchTextBox.Text = "";
            if (SearchByTranslateRadio.IsChecked == true)
            {
                List<WordsPair> result = await wordService.GetByTranslateAsync(textSearch, token);
                _wordsPairAll.Clear();
                foreach (WordsPair w in result)
                {
                    _wordsPairAll.Add(w);
                }
                return;
            }
            if(SearchByWordRadio.IsChecked == true)
            {
                List<WordsPair> result = await wordService.GetByWordAsync(textSearch, token);
                _wordsPairAll.Clear();
                foreach (WordsPair w in result)
                {
                    _wordsPairAll.Add(w);
                }
                return;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is WordsPair wordPair)
                {
                    using CancellationTokenSource cts = new(TimeSpan.FromSeconds(30));
                    CancellationToken token = cts.Token;
                    IWordPairService wordService = _serviceProvider
                        .GetRequiredService<IWordPairService>();
                    await wordService.DeleteAsync(wordPair.Word, wordPair.Translate, token);
                    INotificationService notification = _serviceProvider
                        .GetRequiredService<INotificationService>();
                    notification.ShowSuccess("Пара удалена");
                    LoadData();
                    _totalWords--;
                    TotalWordsCount.Text = _totalWords.ToString();
                }
            }
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            LoadData();
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e) 
        {
            if(_currentPage != 1)
            {
                _currentPage -= 1;
            }
            LoadData();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if(_currentPage < GetLastPage())
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

        private int GetLastPage()
        {
            if (_totalWords <= _sizePage) return 1;
            return (int)Math.Ceiling((double)_totalWords / _sizePage);
        }
    }
}
