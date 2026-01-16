using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.Enums;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class TrainActionView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        private int _currentWord = 0;
        private int _currentStreakWord = 0;
        private int _wordDone = 0;
        private int _variableResponce = 0;
        private string _responseEnter = string.Empty;
        private List<QuestionOption> _questions;
        private List<QuestionEnter> _questionEnter;
        private bool _modeCheckButton;
        private DispatcherTimer _timer;
        private int _seconds = 0;
        public TrainActionView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
            _questions = new List<QuestionOption>();
            _questionEnter = new List<QuestionEnter>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick!;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _seconds++;
            TimerText.Text = TimeFromSeconds(_seconds);
        }

        private string TimeFromSeconds(int seconds)
        {
            int minutes = seconds / 60;
            string result = string.Empty;
            if(minutes < 10)
            {
                result += "0" + minutes.ToString() + ":";
            }
            else
            {
                result += minutes.ToString() + ":";
            }
            if(seconds % 60 < 10)
            {
                result += "0" + (seconds % 60).ToString();
            }
            else
            {
                result += (seconds % 60).ToString();
            }
            return result;
        }

        private void LoadData()
        {
            switch(_appData.Train.Type)
            {
                case TypeQuestion.Enter:
                    OptionPanel.Visibility = Visibility.Collapsed;
                    InputPanel.Visibility = Visibility.Visible;
                    switch(_appData.Train.Mode)
                    {
                        case ModeTrain.WordToTranslate:
                            CreateQuestionEnterWT();
                            break;
                        case ModeTrain.TranslateToWord:
                            CreateQuestionEnterTW();
                            break;
                        case ModeTrain.Mixed:
                            CreateQuestionEnterMixed();
                            break;
                    }
                    break;
                case TypeQuestion.Select:
                    OptionPanel.Visibility = Visibility.Visible;
                    InputPanel.Visibility = Visibility.Collapsed;
                    switch(_appData.Train.Mode)
                    {
                        case ModeTrain.WordToTranslate:
                            CreateQuestionsOptionTW();
                            break;
                        case ModeTrain.TranslateToWord:
                            CreateQuestionsOptionWT();
                            break;
                        case ModeTrain.Mixed:
                            CreateQuestionsOptionMixed();
                            break;
                    }
                    break;
            }
            _currentWord = 0;
            _currentStreakWord = 0;
            _wordDone = 0;
            CurrentWordText.Text = $"{_currentWord + 1} из {_appData.Train.CountWord}";
            CorrectCountText.Text = $"{_wordDone}";
            UpdateProgressBar();
            if(_appData.Train.IsTime)
            {
                _seconds = 0;
                _timer.Start();
            }
            else
            {
                TimerText.Text = "--:--";
            }
        }

        private async void CreateQuestionsOptionWT()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            WordsPair targetPair;
            Random random = new Random();
            _questions.Clear();
            for(int i = 0; i < _appData.Train.CountWord;)
            {
                QuestionOption question = new QuestionOption();
                targetPair = await wordService
                    .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                if (!IsUniqueQuestionOption(targetPair.Word, targetPair.Translate)) continue;
                i++;
                question.SelectWord = targetPair.Word;
                question.CorrectTranslate = targetPair.Translate;
                question.FieldResponse(targetPair.Translate,
                    await wordService.GetTranslateByPositionAsync(
                        random.Next(0, _appData.Stats.CountWords), token),
                    await wordService.GetTranslateByPositionAsync(
                        random.Next(0, _appData.Stats.CountWords), token));
                _questions.Add(question);
            }
        }

        private async void CreateQuestionsOptionTW()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            WordsPair targetPair;
            Random random = new Random();
            _questions.Clear();
            for (int i = 0; i < _appData.Train.CountWord;)
            {
                QuestionOption question = new QuestionOption();
                targetPair = await wordService
                    .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                if (!IsUniqueQuestionOption(targetPair.Word, targetPair.Translate)) continue;
                i++;
                question.SelectWord = targetPair.Translate;
                question.CorrectTranslate = targetPair.Word;
                question.FieldResponse(targetPair.Word,
                    await wordService.GetWordByPositionAsync(
                        random.Next(0, _appData.Stats.CountWords), token),
                    await wordService.GetWordByPositionAsync(
                        random.Next(0, _appData.Stats.CountWords), token));
                _questions.Add(question);
            }
        }

        private async void CreateQuestionsOptionMixed()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            WordsPair targetPair;
            Random random = new Random();
            bool mode = false;
            _questions.Clear();
            for (int i = 0; i < _appData.Train.CountWord;)
            {
                QuestionOption question = new QuestionOption();
                if (mode)
                {
                    targetPair = await wordService
                        .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                    if (!IsUniqueQuestionOption(targetPair.Word, targetPair.Translate)) continue;
                    i++;
                    question.SelectWord = targetPair.Word;
                    question.CorrectTranslate = targetPair.Translate;
                    question.FieldResponse(targetPair.Translate,
                        await wordService.GetTranslateByPositionAsync(
                            random.Next(0, _appData.Stats.CountWords), token),
                        await wordService.GetTranslateByPositionAsync(
                            random.Next(0, _appData.Stats.CountWords), token));
                    _questions.Add(question);
                    mode = false;
                }
                else
                {
                    targetPair = await wordService
                        .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                    if (!IsUniqueQuestionOption(targetPair.Word, targetPair.Translate)) continue;
                    i++;
                    question.SelectWord = targetPair.Translate;
                    question.CorrectTranslate = targetPair.Word;
                    question.FieldResponse(targetPair.Word,
                        await wordService.GetWordByPositionAsync(
                            random.Next(0, _appData.Stats.CountWords), token),
                        await wordService.GetWordByPositionAsync(
                            random.Next(0, _appData.Stats.CountWords), token));
                    _questions.Add(question);
                    mode = true;
                }
            }
        }

        private async void CreateQuestionEnterWT()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            WordsPair targetPair;
            Random random = new Random();
            _questionEnter.Clear();
            for (int i = 0; i < _appData.Train.CountWord;)
            {
                QuestionEnter question = new QuestionEnter();
                targetPair = await wordService
                        .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                if (!IsUniqueQuestionEnter(targetPair.Word, targetPair.Translate)) continue;
                i++;
                question.SelectWord = targetPair.Word;
                question.CorrectTranslate = targetPair.Translate;
                _questionEnter.Add(question);
            }
        }

        private async void CreateQuestionEnterTW()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            WordsPair targetPair;
            Random random = new Random();
            _questionEnter.Clear();
            for (int i = 0; i < _appData.Train.CountWord;)
            {
                QuestionEnter question = new QuestionEnter();
                targetPair = await wordService
                        .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                if(!IsUniqueQuestionEnter(targetPair.Translate, targetPair.Word)) continue;
                i++;
                question.SelectWord = targetPair.Translate;
                question.CorrectTranslate = targetPair.Word;
                _questionEnter.Add(question);
            }
        }

        private async void CreateQuestionEnterMixed()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            WordsPair targetPair;
            bool mode = false;
            Random random = new Random();
            for (int i = 0; i < _appData.Train.CountWord;)
            {
                QuestionEnter question = new QuestionEnter();
                if (mode)
                {
                    targetPair = await wordService
                        .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                    if (!IsUniqueQuestionEnter(targetPair.Word, targetPair.Translate)) continue;
                    i++;
                    question.SelectWord = targetPair.Word;
                    question.CorrectTranslate = targetPair.Translate;
                    _questionEnter.Add(question);
                    mode = false;
                }
                else
                {
                    targetPair = await wordService
                        .GetByPositionAsync(random.Next(0, _appData.Stats.CountWords), token);
                    if (!IsUniqueQuestionEnter(targetPair.Translate, targetPair.Word)) continue;
                    i++;
                    question.SelectWord = targetPair.Translate;
                    question.CorrectTranslate = targetPair.Word;
                    _questionEnter.Add(question);
                    mode = true;
                }
            }
        }

        private bool IsUniqueQuestionOption(string word, string translate)
        {
            foreach (QuestionOption q in _questions)
            {
                if (q.SelectWord == word && q.CorrectTranslate == translate) return false;
            }
            return true;
        }

        private bool IsUniqueQuestionEnter(string word, string translate)
        {
            foreach (QuestionEnter q in _questionEnter)
            {
                if (q.SelectWord == word && q.CorrectTranslate == translate) return false;
            }
            return true;
        }

        private void OptionButtonOne_Click(object sender, RoutedEventArgs e)
        {
            ResetButtons();
            Option1Button.Background = Brushes.Green;
            Option1Button.Foreground = Brushes.White;
            Option1Button.BorderBrush = Brushes.DarkGreen;
            CheckButton.Visibility = Visibility.Visible;
            _variableResponce = 1;
        }

        private void OptionButtonTwo_Click(object sender, RoutedEventArgs e)
        {
            ResetButtons();
            Option2Button.Background = Brushes.Green;
            Option2Button.Foreground = Brushes.White;
            Option2Button.BorderBrush = Brushes.DarkGreen;
            CheckButton.Visibility = Visibility.Visible;
            _variableResponce = 2;
        }

        private void OptionButtonThree_Click(object sender, RoutedEventArgs e)
        {
            ResetButtons();
            Option3Button.Background = Brushes.Green;
            Option3Button.Foreground = Brushes.White;
            Option3Button.BorderBrush = Brushes.DarkGreen;
            CheckButton.Visibility = Visibility.Visible; 
            _variableResponce = 3;
        }

        private void CheckButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if(_modeCheckButton)
            {
                if (_currentWord == _appData.Train.CountWord)
                {
                    _timer.Stop();
                    _appData.TrainResult.CountDone = _wordDone;
                    _appData.TrainResult.Streak = _currentStreakWord;
                    _appData.TrainResult.TrainingTimeSeconds = _seconds;
                    if (_appData.Train.IsTime) TimerText.Text = "00:00";
                    _appData.ChangeViewAction(VariableView.TrainResult);
                    return;
                }
                CurrentWordText.Text = $"{_currentWord + 1} из {_appData.Train.CountWord}";
                CorrectCountText.Text = $"{_wordDone}";
                QuestionText.Text = _questionEnter[_currentWord].SelectWord;
                _modeCheckButton = false;
                CheckInputButton.Content = "Проверить";
                UpdateProgressBar();
                if (CorrectAnswerText.Visibility == Visibility.Visible)
                    CorrectAnswerText.Visibility = Visibility.Collapsed;
                if (PreviousResultText.Visibility == Visibility.Visible)
                {
                    PreviousResultText.Visibility = Visibility.Collapsed;
                    PreviousResultIcon.Visibility = Visibility.Collapsed;
                }
                _responseEnter = string.Empty;
                AnswerTextBox.Text = string.Empty;
            }
            else
            {
                _responseEnter = AnswerTextBox.Text;
                if (string.IsNullOrEmpty(_responseEnter)) return;
                if (_questionEnter[_currentWord].IsDone(_responseEnter))
                {
                    CorrectAnswerText.Visibility = Visibility.Visible;
                    _wordDone++;
                    _currentStreakWord++;
                }
                else
                {
                    _currentStreakWord = 0;
                    PreviousResultIcon.Visibility = Visibility.Visible;
                    PreviousResultText.Visibility = Visibility.Visible;
                }
                _currentWord++;
                _modeCheckButton = true;
                CheckInputButton.Content = "Следующий";
            }
        }

        public void CheckButtonOption_Click(object sender, RoutedEventArgs e)
        {
            if(_modeCheckButton)
            {
                if(_currentWord == _appData.Train.CountWord)
                {
                    _timer.Stop();
                    _appData.TrainResult.CountDone = _wordDone;
                    _appData.TrainResult.Streak = _currentStreakWord;
                    _appData.TrainResult.TrainingTimeSeconds = _seconds;
                    if (_appData.Train.IsTime) TimerText.Text = "00:00";
                    _appData.ChangeViewAction(VariableView.TrainResult);
                    return;
                }
                CurrentWordText.Text = $"{_currentWord + 1} из {_appData.Train.CountWord}";
                CorrectCountText.Text = $"{_wordDone}";
                SetQuestionResponse();
                QuestionText.Text = _questions[_currentWord].SelectWord;
                _modeCheckButton = false;
                CheckButton.Content = "Проверить";
                ResetButtons();
                UpdateProgressBar();
                if (CorrectAnswerText.Visibility == Visibility.Visible)
                    CorrectAnswerText.Visibility = Visibility.Collapsed;
                if (PreviousResultText.Visibility == Visibility.Visible)
                {
                    PreviousResultText.Visibility = Visibility.Collapsed;
                    PreviousResultIcon.Visibility = Visibility.Collapsed;
                }
                _variableResponce = 0;
            }
            else
            {
                if (_variableResponce == 0) return;
                if (_questions[_currentWord].IsDone(_variableResponce))
                {
                    CorrectAnswerText.Visibility = Visibility.Visible;
                    _wordDone++;
                    _currentStreakWord++;
                }
                else
                {
                    _currentStreakWord = 0;
                    PreviousResultIcon.Visibility = Visibility.Visible;
                    PreviousResultText.Visibility = Visibility.Visible;
                }
                _currentWord++;
                _modeCheckButton = true;
                CheckButton.Content = "Следующий";
            }
        }

        private void AnswerTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if(AnswerTextBox.Text.Length > 0)
            {
                CheckInputButton.IsEnabled = true;
                CheckInputButton.Visibility = Visibility.Visible;
            }
            else
            {
                CheckInputButton.IsEnabled = true;
                CheckInputButton.Visibility = Visibility.Collapsed;
            }
        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            if (_appData.Train.IsTime) TimerText.Text = "00:00";
            _appData.ChangeViewAction(VariableView.Train);
        }

        private void SetQuestionResponse()
        {
            Option1Button.Content = _questions[_currentWord].TranslateOne;
            Option2Button.Content = _questions[_currentWord].TranslateTwo;
            Option3Button.Content = _questions[_currentWord].TranslateThree;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            if(_appData.Train.Type == TypeQuestion.Select)
            {
                SetQuestionResponse();
                QuestionText.Text = _questions[_currentWord].SelectWord;
            }
            else
            {
                QuestionText.Text = _questionEnter[_currentWord].SelectWord;
            }
            _modeCheckButton = false;
            TotalCountText.Text = _appData.Train.CountWord.ToString();
        }

        private void ResetButtons()
        {
            Option1Button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2A2A2A")!;
            Option1Button.Foreground = Brushes.White;
            Option1Button.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF555555")!;
            Option2Button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2A2A2A")!;
            Option2Button.Foreground = Brushes.White;
            Option2Button.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF555555")!;
            Option3Button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2A2A2A")!;
            Option3Button.Foreground = Brushes.White;
            Option3Button.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF555555")!;
        } 

        private void UpdateProgressBar()
        {
            double width = ProgressBarSpace.ActualWidth;
            double widthProgres = width / _appData.Train.CountWord * _currentWord + 1;
            ProgressBarFill.Width = widthProgres;
            ProgressText.Text = (100 / _appData.Train.CountWord * _currentWord).ToString() + "%";
        }

        private class QuestionEnter
        {
            public string SelectWord { get; set; } = string.Empty;
            public string CorrectTranslate { get; set; } = string.Empty;

            public bool IsDone(string response)
            {
                if (CorrectTranslate == response.Trim().ToLower()) return true;
                return false;
            }
        }

        private class QuestionOption
        {
            public string SelectWord { get; set; } = string.Empty;
            public string CorrectTranslate {  get; set; } = string.Empty;
            public string TranslateOne { get; set; } = string.Empty;
            public string TranslateTwo { get; set; } = string.Empty;
            public string TranslateThree { get; set; } = string.Empty;
            private Random _random = new Random();

            public bool IsDone(int variableResponse)
            {
                switch (variableResponse)
                {
                    case 1:
                        if (CorrectTranslate == TranslateOne) return true;
                        return false;
                    case 2:
                        if (CorrectTranslate == TranslateTwo) return true;
                        return false;
                    case 3:
                        if (CorrectTranslate == TranslateThree) return true;
                        return false;
                }
                return false;
            }
            public void FieldResponse(string correctTranslate, string translateTwo, string translateThree)
            {
                int varFiels = _random.Next(0, 3);
                switch (varFiels)
                {
                    case 0:
                        TranslateOne = correctTranslate; 
                        TranslateTwo = translateTwo;
                        TranslateThree = translateThree;
                        break;
                    case 1:
                        TranslateOne = translateTwo;
                        TranslateTwo = correctTranslate;
                        TranslateThree = translateThree;
                        break;
                    case 2:
                        TranslateOne = translateTwo;
                        TranslateTwo = translateThree;
                        TranslateThree = correctTranslate;
                        break;
                }
            }
        }
    }
}
