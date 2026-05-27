using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WordRepeat.Abstractions;
using WordRepeat.Application.Abstractions;
using WordRepeat.Application.Services;
using WordRepeat.DataAccess.Sqlite;
using WordRepeat.DataAccess.Sqlite.Abstractions;
using WordRepeat.DataAccess.Sqlite.Repositories;
using WordRepeat.Enums;
using WordRepeat.Models;
using WordRepeat.Services;
using WordRepeat.Views;

namespace WordRepeat
{
    public partial class MainWindow : Window
    {
        private MainView _mainView;
        private WordsView _wordsView;
        private TrainView _trainView;
        private HistoryView _historyView;
        private SettingView _settingView;
        private TrainActionView _trainActionView;
        private TrainResultView _trainResultView;
        private NotesView _notesView;
        private VariableView _currentView = VariableView.Main;
        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        public Action<VariableView> ChangeViewAction { get; private set; }

        private bool _isClosing = false;

        public MainWindow()
        {
            InitializeComponent();

            ChangeViewAction = (view) =>
            {
                _currentView = view;
                ShowViews();
            };

            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<WordRepeatDbContext>(opt =>
                opt.UseSqlite("Data Source=D:\\projects\\projects\\WordRepeat\\WordRepeat\\data.db"));
            _serviceCollection.AddScoped<IHistoryAddRepository, HistoryAddRepository>();
            _serviceCollection.AddScoped<IHistoryTrainRepository, HistoryTrainRepository>();
            _serviceCollection.AddScoped<IHistoryTypesRepository, HistoryTypesRepository>();
            _serviceCollection.AddScoped<INotesRepository, NotesRepository>();
            _serviceCollection.AddScoped<IWordsPairRepository, WordsPairRepository>();
            _serviceCollection.AddScoped<IHistoryAddServices, HistoryAddServices>();
            _serviceCollection.AddScoped<IHistoryTrainService, HistoryTrainService>();
            _serviceCollection.AddScoped<IHistoryTypesService, HistoryTypesService>();
            _serviceCollection.AddScoped<INotesService, NotesService>();
            _serviceCollection.AddScoped<IWordPairService, WordPairService>();
            _serviceCollection.AddScoped<INotificationService>(pr =>
                new NotificationService(NotificationContainer));
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            _appData = new AppData(ChangeViewAction, new(), new(), new());
            _mainView = new MainView(_serviceProvider, _appData);
            _wordsView = new WordsView(_serviceProvider, _appData);
            _trainView = new TrainView(_serviceProvider, _appData);
            _historyView = new HistoryView(_serviceProvider, _appData);
            _settingView = new SettingView(_serviceProvider, _appData);
            _trainActionView = new TrainActionView(_serviceProvider, _appData);
            _trainResultView = new TrainResultView(_serviceProvider, _appData);
            _notesView = new NotesView(_serviceProvider, _appData);

            CreateAppData();
            ShowViews();
            SizeChanged += MainWindow_SizeChanged;
            StateChanged += MainWindow_StateChanged!;

            SetupWindowButtons();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayOpenAnimation();
        }

        private void PlayOpenAnimation()
        {
            if (MainBorder != null)
            {
                MainBorder.Opacity = 0;

                var fadeAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                MainBorder.BeginAnimation(OpacityProperty, fadeAnimation);

                var scaleTransform = new ScaleTransform(0.98, 0.98);
                MainBorder.RenderTransform = scaleTransform;
                MainBorder.RenderTransformOrigin = new Point(0.5, 0.5);

                var scaleXAnimation = new DoubleAnimation(0.98, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                var scaleYAnimation = new DoubleAnimation(0.98, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
            }
        }

        private void PlayCloseAnimation()
        {
            if (_isClosing) return;
            _isClosing = true;

            if (MainBorder != null)
            {
                var fadeAnimation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };

                var scaleTransform = MainBorder.RenderTransform as ScaleTransform;
                if (scaleTransform != null)
                {
                    var scaleXAnimation = new DoubleAnimation(1, 0.98, TimeSpan.FromMilliseconds(200))
                    {
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                    };

                    var scaleYAnimation = new DoubleAnimation(1, 0.98, TimeSpan.FromMilliseconds(200))
                    {
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
                }

                fadeAnimation.Completed += (s, e) =>
                {
                    System.Windows.Application.Current.Shutdown();
                };

                MainBorder.BeginAnimation(OpacityProperty, fadeAnimation);
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private async void AnimateWindowStateChange()
        {
            if (MainBorder != null)
            {
                var animation = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(200))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                MainBorder.Opacity = 0.7;
                MainBorder.BeginAnimation(OpacityProperty, animation);

                await Task.Delay(200);
                MainBorder.Opacity = 1;
            }
        }

        private void SetupWindowButtons()
        {
            if (WindowState == WindowState.Maximized)
            {
                MaximizeButton.Content = "\uE923";
                if (MainBorder != null)
                {
                    MainBorder.CornerRadius = new CornerRadius(0);
                }
            }
            else
            {
                MaximizeButton.Content = "\uE922"; 
            }
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MaximizeButton.Content = "\uE923"; 
                if (MainBorder != null)
                {
                    MainBorder.CornerRadius = new CornerRadius(0);
                }
            }
            else if (WindowState == WindowState.Normal)
            {
                MaximizeButton.Content = "\uE922"; 
                if (MainBorder != null)
                {
                    MainBorder.CornerRadius = new CornerRadius(10);
                }
            }

            AnimateWindowStateChange();
        }

        public async void CreateAppData()
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;
            IWordPairService wordService = _serviceProvider.GetRequiredService<IWordPairService>();
            _appData.Stats.CountWords = await wordService.CountAsync(token);
        }

        private void MainButtonClick(object sender, RoutedEventArgs e)
        {
            DisableView();
            _currentView = VariableView.Main;
            MainButton.Background = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            ShowViews();
        }

        private void WordButtonClick(object sender, RoutedEventArgs e)
        {
            DisableView();
            _currentView = VariableView.Words;
            WordButton.Background = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            ShowViews();
        }

        private void TrainButtonClick(object sender, RoutedEventArgs e)
        {
            DisableView();
            _currentView = VariableView.Train;
            TrainButton.Background = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            ShowViews();
        }

        private void NotesButtonClick(object sender, RoutedEventArgs e)
        {
            DisableView();
            _currentView = VariableView.Notes;
            NotesButton.Background = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            ShowViews();
        }

        private void HistoryButtonClick(object sender, RoutedEventArgs e)
        {
            DisableView();
            _currentView = VariableView.History;
            HistoryButton.Background = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            ShowViews();
        }

        private void SettingButtonClick(object sender, RoutedEventArgs e)
        {
            DisableView();
            _currentView = VariableView.Setting;
            SettingButton.Background = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            ShowViews();
        }

        private void DisableView()
        {
            switch (_currentView)
            {
                case VariableView.Main:
                    MainButton.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    break;
                case VariableView.Words:
                    WordButton.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    break;
                case VariableView.Train:
                    TrainButton.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    break;
                case VariableView.Notes:
                    NotesButton.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    break;
                case VariableView.History:
                    HistoryButton.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    break;
                case VariableView.Setting:
                    SettingButton.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
                    break;
                default:
                    break;
            }
        }

        private void ShowViews()
        {
            switch (_currentView)
            {
                case VariableView.Main:
                    MainContentControl.Content = _mainView;
                    break;
                case VariableView.Words:
                    MainContentControl.Content = _wordsView;
                    break;
                case VariableView.Train:
                    MainContentControl.Content = _trainView;
                    break;
                case VariableView.History:
                    MainContentControl.Content = _historyView;
                    break;
                case VariableView.Setting:
                    MainContentControl.Content = _settingView;
                    break;
                case VariableView.TrainAction:
                    MainContentControl.Content = _trainActionView;
                    break;
                case VariableView.TrainResult:
                    MainContentControl.Content = _trainResultView;
                    break;
                case VariableView.Notes:
                    MainContentControl.Content = _notesView;
                    break;
                default:
                    break;
            }

            if (MainContentControl.Content is FrameworkElement content)
            {
                content.Opacity = 0;
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                content.BeginAnimation(OpacityProperty, fadeIn);
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainContentControl.Content is FrameworkElement content)
            {
                content.Width = MainContentControl.ActualWidth;
                content.Height = MainContentControl.ActualHeight;
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private async void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainBorder != null)
            {
                var animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };

                animation.Completed += (s, _) =>
                {
                    this.WindowState = WindowState.Minimized;
                    MainBorder.Opacity = 1;
                };

                MainBorder.BeginAnimation(OpacityProperty, animation);
                await Task.Delay(150);
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            PlayCloseAnimation();
        }

        private void ToggleMaximize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
    }
}