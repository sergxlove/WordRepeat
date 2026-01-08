using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordRepeat.Views;

namespace WordRepeat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainView _mainView;
        private WordsView _wordsView;
        private TrainView _trainView;
        private HistoryView _historyView;
        private SettingView _settingView;
        private VariableView _currentView = VariableView.None;

        public MainWindow()
        {
            InitializeComponent();
            _mainView = new MainView();
            _wordsView = new WordsView();
            _trainView = new TrainView();
            _historyView = new HistoryView();
            _settingView = new SettingView();
            SizeChanged += MainWindow_SizeChanged;
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
                    MainButton.Background = new SolidColorBrush(Color.FromRgb(10, 10, 15));
                    break;
                case VariableView.Words:
                    WordButton.Background = new SolidColorBrush(Color.FromRgb(10, 10, 15));
                    break;
                case VariableView.Train:
                    TrainButton.Background = new SolidColorBrush(Color.FromRgb(10, 10, 15));
                    break;
                case VariableView.History:
                    HistoryButton.Background = new SolidColorBrush(Color.FromRgb(10, 10, 15));
                    break;
                case VariableView.Setting:
                    SettingButton.Background = new SolidColorBrush(Color.FromRgb(10, 10, 15));
                    break;
                default:
                    break;
            }
        }

        private void ShowViews()
        {
            switch(_currentView)
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
                default:
                    break;
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

        private enum VariableView
        {
            None = 0,
            Main = 1,
            Words = 2, 
            Train = 3, 
            History = 4,
            Setting = 5
        }
    }

}