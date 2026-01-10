using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Abstractions;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Infrastructures;
using WordRepeat.Core.Models;

namespace WordRepeat.Views
{
    public partial class WordsView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private ObservableCollection<WordsPair> _wordsPairAll;
        private int _totalWords;
        private int _currentPage = 1;
        private int _sizePage = 50;
        public WordsView(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _wordsPairAll = new ObservableCollection<WordsPair>();
            WordsDataGrid.ItemsSource = _wordsPairAll;
            _totalWords = Convert.ToInt32(PageSizeComboBox.Text);
            LoadData();
        }

        private async void LoadData()
        {
            _wordsPairAll.Clear();
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            List<WordsPair> result = await wordService
                .GetByPaginationAsync(_currentPage, _sizePage, token);
            foreach (WordsPair w in result)
            {
                _wordsPairAll.Add(w);
            }
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
            WordTextBox.Text = "";
            TranslationTextBox.Text = "";
        }

        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            
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
                }
            }
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e) 
        {

        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
