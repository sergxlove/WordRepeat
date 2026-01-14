using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Abstractions;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Infrastructures;
using WordRepeat.Core.Models;
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

        private async void ImportCard_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            INotificationService notification = _serviceProvider
                .GetRequiredService<INotificationService>();
            try
            {
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                CancellationToken token = cts.Token;
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Выберите файл",
                    Filter = "Текстовые файлы (*.txt)|*.txt",
                    Multiselect = false,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };
                if (openFileDialog.ShowDialog() == true)
                {

                    string filePath = openFileDialog.FileName;
                    string[] lines = await File.ReadAllLinesAsync(filePath);
                    string[] pair;
                    IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
                    ResultCreateModel<WordsPair> wordPair;
                    foreach (string line in lines)
                    {
                        pair = line.Split(' ');
                        if (string.IsNullOrEmpty(pair[0]) || string.IsNullOrEmpty(pair[1])) continue;
                        wordPair = WordsPair.Create(pair[0], pair[1]);
                        if (!string.IsNullOrEmpty(wordPair.Error)) continue;
                        await wordService.AddAsync(wordPair.Value, token);
                    }
                }
                notification.ShowSuccess("Данные успешно импортированы");
            }
            catch
            {
                notification.ShowError("Произошла ошибка");
            }
        }

        private async void ExportCard_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            INotificationService notification = _serviceProvider
                .GetRequiredService<INotificationService>();
            try
            {
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                CancellationToken token = cts.Token;
                string filePath = "D:\\projects\\projects\\WordRepeat\\WordRepeat\\words.txt";
                IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
                int count = await wordService.CountAsync(token);
                WordsPair pairs;
                for (int i = 0; i < count; i++)
                {
                    pairs = await wordService.GetByPositionAsync(i, token);
                    File.AppendAllText(filePath, $"{pairs.Word} {pairs.Translate}\n");
                }
                notification.ShowSuccess("Данные успешно экспортированы");
            }
            catch
            {
                notification.ShowError("Произошла ошибка");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WordsCountText.Text = _appData.Stats.CountWords.ToString();
        }
    }
}
