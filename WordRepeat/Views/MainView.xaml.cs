using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WordRepeat.Application.Abstractions;
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

        public async void LoadData()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            IHistoryAddServices addService = _serviceProvider.GetRequiredService<IHistoryAddServices>();
            IHistoryTrainService trainService = _serviceProvider.GetRequiredService<IHistoryTrainService>();
            TotalWordsText.Text = Convert.ToString(await wordService.CountAsync(token));
            TodayLearnedText.Text = Convert.ToString(await addService.GetAddedTodayAsync(token));
            TodayRepeatedText.Text = Convert.ToString(await trainService.GetTrainedTodayAsync(token));
            AccuracyText.Text = Convert.ToString(await trainService.GetAccuracyByWeekAsync(token)) + "%";
            OverallAccuracyText.Text = Convert.ToString(await trainService.GetAccuracyByAllAsync(token)) + "%";
            WeeklyAccuracyText.Text = AccuracyText.Text;
            MonthlyAccuracyText.Text = Convert.ToString(await trainService.GetAccuracyByMonthAsync(token)) + "%";
            CorrectAnswersText.Text = $"Правильные ответы: {await trainService.GetCountDoneAsync(token)}";
            WrongAnswersText.Text = $"Неправильные ответы: {await trainService.GetCountWrongAsync(token)}";
            TotalSessionsText.Text = $"Всего тренировок: {await trainService.CountAsync(token)}";
            Achievement2Description.Text = $"Повторили {await trainService.GetTrainedTodayAsync(token)} слов";
            Achievement1Description.Text = $"Занимались {await trainService.GetStreakAsync(token)} дней подряд";
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
