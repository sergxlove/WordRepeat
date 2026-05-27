using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using WordRepeat.Abstractions;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
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
            MotivationPanel.Text = Motivations.GetMotivation();
            var weeklyActivity = await GetWeeklyActivityAsync(token);
            DailyStatsDataGrid.ItemsSource = weeklyActivity;
        }
        
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch
            {
                INotificationService notification = _serviceProvider
                    .GetRequiredService<INotificationService>();
                notification.ShowError("Произошла ошибка");
            }
        }

        private async Task<List<DailyActivityModel>> GetWeeklyActivityAsync(CancellationToken token)
        {
            List<DailyActivityModel> result = new();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            IHistoryTrainService historyTrainService = _serviceProvider
                .GetRequiredService<IHistoryTrainService>();
            IHistoryAddServices historyAddService = _serviceProvider 
                .GetRequiredService<IHistoryAddServices>();
            for (int i = 0; i < 7; i++)
            {
                var date = today.AddDays(-i);
                HistoryTrain? trainDay = await historyTrainService.GetByDateAsync(date, token);
                HistoryAdd? addDay = await historyAddService.GetByDateAsync(date, token);
                int accuracyDay = await historyTrainService.GetAccuracyByDayAsync(date, token);
                string dayName;
                if (i == 0)
                    dayName = "Сегодня";
                else if (i == 1)
                    dayName = "Вчера";
                else
                    dayName = date.ToString("dddd", new System.Globalization.CultureInfo("ru-RU"));
                int trainDayCount;
                int addDayCount;
                if (trainDay is null) trainDayCount = 0;
                else trainDayCount = trainDay.Total;
                if (addDay is null) addDayCount = 0;
                else addDayCount = addDay.CountAdd;
                result.Add(new DailyActivityModel
                {
                    Day = dayName,
                    AddedWords = addDayCount,
                    RepeatedWords = trainDayCount,
                    Accuracy = accuracyDay,
                    AccuracyColor = GetAccuracyColor(accuracyDay)
                });
            }
            return result;
        }

        private System.Windows.Media.Brush GetAccuracyColor(double accuracy)
        {
            if (accuracy >= 80)
                return System.Windows.Media.Brushes.LightGreen;
            if (accuracy >= 60)
                return System.Windows.Media.Brushes.Gold;
            return System.Windows.Media.Brushes.OrangeRed;
        }
    }
}
