using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

        private async void ImportWordsButton_Click(object sender, RoutedEventArgs e)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            string filePath = "D:\\projects\\projects\\WordRepeat\\WordRepeat\\words.txt";
            string[] lines = await File.ReadAllLinesAsync(filePath);
            string[] pair;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            ResultCreateModel<WordsPair> wordPair;
            foreach (string line in lines)
            {
                pair = line.Split(' ');
                if (string.IsNullOrEmpty(pair[0]) || string.IsNullOrEmpty(pair[1])) continue;
                wordPair = WordsPair.Create(pair[0], pair[1]);
                if (string.IsNullOrEmpty(wordPair.Error)) continue;
                await wordService.AddAsync(wordPair.Value, token);
            }
        }

        private void ExportWordsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
